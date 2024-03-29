﻿namespace Nexus.Client.ModManagement
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Nexus.Client.BackgroundTasks;
	using Nexus.Client.ModRepositories;
	using Nexus.Client.Mods;
	using Nexus.Client.UI;

	public class ModUpdateCheckTask : ThreadedBackgroundTask
	{
		private readonly bool _overrideCategorySetup;
		private readonly bool? _missingDownloadId = false;
		private readonly List<IMod> _modList = new List<IMod>();
		private readonly bool _overrideLocalModNames;
		private readonly Dictionary<string, string> _newDownloadID = new Dictionary<string, string>();
		private int _retries = 0;
		private bool _cancel;
		private string _period = string.Empty;

		#region Properties

		/// <summary>
		/// Gets the AutoUpdater.
		/// </summary>
		/// <value>The AutoUpdater.</value>
		protected AutoUpdater AutoUpdater { get; }

		/// <summary>
		/// Gets the current mod repository.
		/// </summary>
		/// <value>The current mod repository.</value>
		protected IModRepository ModRepository { get; }

		/// <summary>
		/// Gets the current profile manager.
		/// </summary>
		/// <value>The current profile manager.</value>
		protected IProfileManager ProfileManager { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// A simple constructor that initializes the object with its dependencies.
		/// </summary>
		/// <param name="autoUpdater">The AutoUpdater.</param>
		/// <param name="modRepository">The current mod repository.</param>
		/// <param name="modList">The list of mods we need to update.</param>
		/// <param name="overrideCategorySetup">Whether to force a global update.</param>
		/// <inheritdoc />
		public ModUpdateCheckTask(AutoUpdater autoUpdater, IProfileManager profileManager, IModRepository modRepository, IEnumerable<IMod> modList, string period, bool overrideCategorySetup, bool? missingDownloadId, bool overrideLocalModNames)
		{
			AutoUpdater = autoUpdater;
			ModRepository = modRepository;
			ProfileManager = profileManager;
			_period = period;
			_modList.AddRange(modList);
			_overrideCategorySetup = overrideCategorySetup;
			_missingDownloadId = missingDownloadId;
			_overrideLocalModNames = overrideLocalModNames;
		}

		#endregion

		/// <summary>
		/// Starts the update.
		/// </summary>
		/// <param name="confirm">The delegate to call to confirm an action.</param>
		public void Update(ConfirmActionMethod confirm)
		{
			Start(confirm);
		}

		/// <summary>
		/// Cancels the update.
		/// </summary>
		/// <inheritdoc />
		public override void Cancel()
		{
			base.Cancel();
			_cancel = true;
		}

		/// <summary>
		/// The method that is called to start the background task.
		/// </summary>
		/// <param name="args">Arguments to for the task execution.</param>
		/// <returns>Always null.</returns>
		protected override object DoWork(object[] args)
		{
			List<string> updatedMods = new List<string>();

			var modList = new List<string>();
			var modCheck = new List<IMod>();

			OverallMessage = "Updating mods info: setup search..";
			OverallProgress = 0;
			OverallProgressStepSize = 1;
			ShowItemProgress = true;
			ItemProgress = 0;
			ItemProgressStepSize = 1;
			ItemProgressMaximum = 1;
			OverallProgressMaximum = 1;

			OverallProgressMaximum = _modList.Count * 2;
			ItemProgressMaximum = _modList.Count;

			if (!string.IsNullOrEmpty(_period))
			{
				// Get updated mods in the chosen period
				updatedMods = ModRepository.GetUpdated(_period);
			}

			for (int i = 0; i < _modList.Count; i++)
			{
				if (_cancel)
				{
					break;
				}

				IMod modCurrent = _modList[i];
				string modId = string.Empty;
				int isEndorsed = 0;
				ItemMessage = modCurrent.ModName;

				string modName = StripFileName(modCurrent.Filename, modCurrent.Id);

				// If the know the modId we save the current Endorsment status. Otherwise we search for the mod Id on the Nexus.
				if (!string.IsNullOrEmpty(modCurrent.Id))
				{
					modId = modCurrent.Id;
					isEndorsed = modCurrent.IsEndorsed == true ? 1 : modCurrent.IsEndorsed == false ? -1 : 0;
				}
				else if (_missingDownloadId == null || _missingDownloadId == true)
				{
					// Deprecated, too many useless request were being thrown, if users need to recognize a mod they can just use the mod tagger.
					// It is just active for the specific downloadId search.
					IModInfo modInfoForFile = ModRepository.GetModInfoForFile(modCurrent.Filename);

					if (modInfoForFile != null)
					{
						modCurrent.Id = modInfoForFile.Id;
						modId = modInfoForFile.Id;
						AutoUpdater.AddNewVersionNumberForMod(modCurrent, modInfoForFile);
						modName = StripFileName(modCurrent.Filename, modInfoForFile.Id);
					}
				}

				// If we're looking for missing download Ids
				if (_missingDownloadId == null || _missingDownloadId == true && (string.IsNullOrEmpty(modCurrent.DownloadId) || modCurrent.DownloadId == "0" || modCurrent.DownloadId == "-1"))
				{
					if (updatedMods.Count > 0 && !string.IsNullOrWhiteSpace(modId) && updatedMods.Contains(modId, StringComparer.OrdinalIgnoreCase))
					{
						modList.Add(string.Format("{0}|{1}|{2}|{3}", modName, modId, modCurrent.DownloadId, Path.GetFileName(modCurrent.Filename)));
						modCheck.Add(modCurrent);
					}
					else if (updatedMods.Count == 0 || string.IsNullOrWhiteSpace(modId))
					{
						modList.Add(string.Format("{0}|{1}|{2}|{3}", modName, string.IsNullOrWhiteSpace(modId) ? "0" : modId, string.IsNullOrWhiteSpace(modCurrent.DownloadId) ? "0" : modCurrent.DownloadId, Path.GetFileName(modCurrent.Filename)));
						modCheck.Add(modCurrent);
					}
				}
				//  If we're performing a period-based update check or an update after a category reset
				else if (_missingDownloadId == false && !string.IsNullOrEmpty(modId))
				{
					// If we're performing a category reset
					if (_overrideCategorySetup && !string.IsNullOrEmpty(modCurrent.DownloadId))
					{
						// TODO: This used to work with download ids, it requires a new method in the API, currently it did nothing cause it passed downloadId to a modId search.
						modList.Add(string.Format("{0}|{1}", string.IsNullOrWhiteSpace(modId) ? "0" : modId, modCurrent.DownloadId));
						modCheck.Add(modCurrent);
					}
					// If we're performing a period-based update check
					else if (updatedMods.Count > 0 && !string.IsNullOrWhiteSpace(modId) && updatedMods.Contains(modId, StringComparer.OrdinalIgnoreCase))
					{
						modList.Add(string.Format("{0}|{1}|{2}|{3}", modName, string.IsNullOrWhiteSpace(modId) ? "0" : modId, string.IsNullOrWhiteSpace(modCurrent.DownloadId) ? "0" : modCurrent.DownloadId, Path.GetFileName(modCurrent.Filename)));
						modCheck.Add(modCurrent);
					}
				}

				if (ItemProgress < ItemProgressMaximum)
					StepItemProgress();
				if (OverallProgress < OverallProgressMaximum)
					StepOverallProgress();

				if (_cancel)
					break;

				// Prevents the repository request string from becoming too long.
				// Since the system has been overhauled this is no longer required.
				//if (modList.Count == modLimit)
				//{
				//	string strResult = CheckForModListUpdate(modList, modCheck);

				//	if (!string.IsNullOrEmpty(strResult))
				//	{
				//		modList.Clear();
				//		return strResult;
				//	}

				//	modList.Clear();
				//	OverallMessage = "Updating mods info: setup search..";
				//	ItemProgress = 0;
				//	ItemProgressMaximum = _modList.Count == modLimit ? 1 : _modList.Count - (i + 1);
				//}
			}

			if (!_cancel && modList.Count > 0)
			{
				string strResult = CheckForModListUpdate(modList, modCheck);

				if (!string.IsNullOrEmpty(strResult))
				{
					_modList.Clear();
					return strResult;
				}
			}

			_modList.Clear();

			return _newDownloadID;
		}

		private string StripFileName(string p_strFileName, string p_strId)
		{
			string strModFilename = string.Empty;

			if (!string.IsNullOrWhiteSpace(p_strFileName))
			{
				strModFilename = Path.GetFileNameWithoutExtension(p_strFileName);

				if (!string.IsNullOrWhiteSpace(p_strId))
				{
					if (p_strId.Length > 2 && strModFilename.IndexOf(p_strId) > 0)
					{
						string strModIDPattern = "-" + p_strId + "-";
						string strVersionlessPattern = "-" + p_strId;

						if (strModFilename.IndexOf(strModIDPattern, 0) > 0)
							strModFilename = strModFilename.Substring(0, strModFilename.IndexOf(strModIDPattern, 0));
						else if (strModFilename.IndexOf(strVersionlessPattern, 0) > 0)
							strModFilename = strModFilename.Substring(0, strModFilename.IndexOf(strVersionlessPattern, 0));
					}
					else
					{
						if (strModFilename.IndexOf('-', 0) > 0)
							strModFilename = strModFilename.Substring(0, strModFilename.IndexOf('-', 0));
					}
				}
			}
			return strModFilename.Trim();
		}

		/// <summary>
		/// Checks for the updated information for the given mods.
		/// </summary>
		/// <param name="modList">The mods for which to check for updates.</param>
		private string CheckForModListUpdate(List<string> modList, List<IMod> modsToCheck)
		{
			OverallMessage = _missingDownloadId != false ? "Updating mods info: retrieving download ids.." : "Updating mods info: getting online updates..";
			List<IModInfo> fileListInfo = new List<IModInfo>();
			IMod[] modCheckList = modsToCheck.ToArray();

			//get mod info
			for (int i = 0; i <= _retries; i++)
			{
				fileListInfo = ModRepository.GetFileListInfo(modList);

				if (fileListInfo != null)
				{
					break;
				}

				Task.Delay(2500);
			}

			if (fileListInfo != null)
			{
				IModInfo[] modUpdates = fileListInfo.ToArray();
				ItemProgress = 0;
				ItemProgressMaximum = fileListInfo.Count;

				for (int i = 0; i < modUpdates.Count(); i++)
				{
					ModInfo modUpdate = (ModInfo)modUpdates[i];
					if (_cancel)
					{
						break;
					}

					if (OverallProgress < OverallProgressMaximum)
					{
						StepOverallProgress();
					}

					if (modUpdate == null)
					{
						continue;
					}

					ItemMessage = modUpdate.ModName;

					// Increasing readability
					IMod mod = null;

					if (_missingDownloadId == false)
					{
						mod = _modList.Where(x => x != null).FirstOrDefault(m => !string.IsNullOrEmpty(modUpdate.FileName) &&
							modUpdate.FileName.Equals(Path.GetFileName(m.Filename), StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						mod = _modList.Where(x => x != null).FirstOrDefault(x => !string.IsNullOrEmpty(modUpdate.FileName) && (StripFileName(modUpdate.FileName, modUpdate.Id).Equals(StripFileName(Path.GetFileName(x.Filename).ToString(), x.Id), StringComparison.OrdinalIgnoreCase) ||
							StripFileName(modUpdate.FileName, modUpdate.Id).Equals(StripFileName(Path.GetFileName(x.Filename.Replace("_", " ")).ToString(), x.Id), StringComparison.OrdinalIgnoreCase)));
					}

					if (mod == null && !string.IsNullOrEmpty(modUpdate.DownloadId) && modUpdate.DownloadId != "0" && modUpdate.DownloadId != "-1")
					{
						mod = _modList.Where(x => x != null).FirstOrDefault(x => !string.IsNullOrEmpty(x.DownloadId) && modUpdate.DownloadId.Equals(x.DownloadId, StringComparison.OrdinalIgnoreCase));
					}

					if (mod == null)
					{
						if (_missingDownloadId != false)
						{
							var modCheck = modCheckList[i];
							if (!string.IsNullOrEmpty(modUpdate.Id) && modUpdate.Id != "0" && !string.IsNullOrEmpty(modCheck.Id) && modCheck.Id != "0")
							{
								if (modUpdate.Id.Equals(modCheck.Id, StringComparison.OrdinalIgnoreCase))
								{
									mod = modCheck;
								}
							}
						}
					}

					if (mod != null)
					{
						if (ItemProgress < ItemProgressMaximum)
						{
							StepItemProgress();
						}

						if (modUpdate.DownloadId == "-1")
						{
							if (_missingDownloadId != false)
							{
								var filename = Path.GetFileName(mod.Filename);

								if (!string.Equals(StripFileName(filename, mod.Id), StripFileName(modUpdate.FileName, modUpdate.Id), StringComparison.InvariantCultureIgnoreCase))
								{
									continue;
								}
							}
						}
						else if (!string.IsNullOrWhiteSpace(modUpdate.DownloadId) && !modUpdate.DownloadId.Equals(mod.DownloadId))
						{
							if (_missingDownloadId != false)
							{
								var filename = Path.GetFileName(mod.Filename);

								if (string.Equals(filename, modUpdate.FileName, StringComparison.InvariantCultureIgnoreCase))
								{
									if (!_newDownloadID.ContainsKey(filename))
									{
										_newDownloadID.Add(filename, modUpdate.DownloadId);
									}
								}
								else if (!string.IsNullOrEmpty(mod.Id) && mod.Id != "0" && !string.IsNullOrEmpty(modUpdate.Id) && modUpdate.Id != "0")
								{
									if (string.Equals(mod.Id, modUpdate.Id, StringComparison.InvariantCultureIgnoreCase) && !_newDownloadID.ContainsKey(filename))
									{
										_newDownloadID.Add(filename, modUpdate.DownloadId);
									}
								}
							}
						}

						if (!string.IsNullOrEmpty(mod.DownloadId) && string.IsNullOrWhiteSpace(modUpdate.DownloadId))
						{
							modUpdate.DownloadId = mod.DownloadId;
						}

						if (_missingDownloadId != false)
						{
							modUpdate.HumanReadableVersion = !string.IsNullOrEmpty(mod.HumanReadableVersion) ? mod.HumanReadableVersion : modUpdate.HumanReadableVersion;
							modUpdate.MachineVersion = mod.MachineVersion != null ? mod.MachineVersion : modUpdate.MachineVersion;
						}

						if (mod.CustomCategoryId != 0 && mod.CustomCategoryId != -1)
						{
							modUpdate.CustomCategoryId = mod.CustomCategoryId;
						}

						modUpdate.UpdateWarningEnabled = mod.UpdateWarningEnabled;
						modUpdate.UpdateChecksEnabled = mod.UpdateChecksEnabled;
						AutoUpdater.AddNewVersionNumberForMod(mod, modUpdate);

						if (!_overrideLocalModNames)
						{
							modUpdate.ModName = mod.ModName;
						}

						if (!string.IsNullOrEmpty(mod.ModName))
							modUpdate.ModName = string.Empty;
						mod.UpdateInfo(modUpdate, null);
						ItemProgress = 0;
					}
				}

				if (modUpdates.Count() < modCheckList.Count())
					Cancel();
			}

			return null;
		}
	}
}
