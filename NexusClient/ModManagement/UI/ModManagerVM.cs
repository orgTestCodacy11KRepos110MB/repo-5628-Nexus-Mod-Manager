﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Nexus.Client.BackgroundTasks;
using Nexus.Client.BackgroundTasks.UI;
using Nexus.Client.Commands.Generic;
using Nexus.Client.Games;
using Nexus.UI.Controls;
using Nexus.Client.ModManagement;
using Nexus.Client.ModRepositories;
using Nexus.Client.Mods;
using Nexus.Client.Settings;
using Nexus.Client.UI;
using Nexus.Client.Util;
using Nexus.Client.Util.Collections;

namespace Nexus.Client.ModManagement.UI
{
	/// <summary>
	/// This class encapsulates the data and the operations presented by UI
	/// elements that display mod management.
	/// </summary>
	public class ModManagerVM
	{
		private bool m_booIsCategoryInitialized = false;
		private Control m_ctlParentForm = null;

		#region Events

		/// <summary>
		/// Raised when mods are about to be added to the mod manager.
		/// </summary>
		public event EventHandler<EventArgs<IBackgroundTask>> AddingMod = delegate { };

		/// <summary>
		/// Raised when mods are about to be deleted from the mod manager.
		/// </summary>
		public event EventHandler<EventArgs<IBackgroundTaskSet>> DeletingMod = delegate { };

		/// <summary>
		/// Raised when the activation status of a mod is about to be changed.
		/// </summary>
		public event EventHandler<EventArgs<IBackgroundTaskSet>> ChangingModActivation = delegate { };

		/// <summary>
		/// Raised when a mod is being tagged.
		/// </summary>
		public event EventHandler<EventArgs<ModTaggerVM>> TaggingMod = delegate { };

		/// <summary>
		/// Raised when the category list is being updated.
		/// </summary>
		public event EventHandler<EventArgs<IBackgroundTask>> UpdatingCategory = delegate { };

		/// <summary>
		/// Raised when the mods list is being updated.
		/// </summary>
		public event EventHandler<EventArgs<IBackgroundTask>> UpdatingMods = delegate { };

		/// <summary>
		/// Raised when the readme manager is being set up.
		/// </summary>
		public event EventHandler<EventArgs<IBackgroundTask>> ReadMeManagerSetup = delegate { };


		/// <summary>
		/// Raised when the toggle all mods is being set up.
		/// </summary>
		public event EventHandler<EventArgs<IBackgroundTask>> TogglingAllWarning = delegate { };

		/// <summary>
		/// Raised when uninstalling multiple mods.
		/// </summary>
		public event EventHandler<EventArgs<IBackgroundTask>> DeactivatingMultipleMods = delegate { };

		#endregion

		#region Delegates

		/// <summary>
		/// Called when an updater's action needs to be confirmed.
		/// </summary>
		public ConfirmActionMethod ConfirmUpdaterAction = delegate { return true; };

		#endregion

		/// <summary>
		/// Raised when the deletion of a mod file needs to be confirmed.
		/// </summary>
		public Func<IMod, bool> ConfirmModFileDeletion = delegate { return false; };

		/// <summary>
		/// Raised when the overwriting of a mod file needs to be confirmed.
		/// </summary>
		public Func<string, string, string> ConfirmModFileOverwrite = delegate { return null; };

		/// <summary>
		/// Raised when the overwriting of an item by another item being installed by a mod needs to be confirmed.
		/// </summary>
		public ConfirmItemOverwriteDelegate ConfirmItemOverwrite = delegate { return OverwriteResult.No; };

		/// <summary>
		/// Raised when the overwriting of an item by another item being installed by a mod needs to be confirmed.
		/// </summary>
		public ConfirmModUpgradeDelegate ConfirmModUpgrade = delegate { return ConfirmUpgradeResult.Cancel; };

		#region Properties

		#region Commands

		/// <summary>
		/// Gets the command to add a mod to the manager.
		/// </summary>
		/// <remarks>
		/// The commands takes an argument describing the file path to the mod to be added.
		/// </remarks>
		/// <value>The command to add a mod to the manager.</value>
		public Command<string> AddModCommand { get; private set; }

		/// <summary>
		/// Gets the command to delete a mod from the manager.
		/// </summary>
		/// <remarks>
		/// The commands takes an argument describing the mod to be deleted.
		/// </remarks>
		/// <value>The command to delete a mod from the manager.</value>
		public Command<IMod> DeleteModCommand { get; private set; }

		/// <summary>
		/// Gets the command to activate a mod.
		/// </summary>
		/// <remarks>
		/// The commands takes an argument describing the mod to be activated.
		/// </remarks>
		/// <value>The command to activate a mod.</value>
		public Command<IMod> ActivateModCommand { get; private set; }

		/// <summary>
		/// Gets the command to deactivate a mod.
		/// </summary>
		/// <remarks>
		/// The commands takes an argument describing the mod to be deactivated.
		/// </remarks>
		/// <value>The command to deactivate a mod.</value>
		public Command<IMod> DeactivateModCommand { get; private set; }

		/// <summary>
		/// Gets the command to tag a mod.
		/// </summary>
		/// <remarks>
		/// The commands takes an argument describing the mod to be tagged.
		/// </remarks>
		/// <value>The command to tag a mod.</value>
		public Command<IMod> TagModCommand { get; private set; }

		#endregion

		/// <summary>
		/// Gets the mod manager to use to manage mods.
		/// </summary>
		/// <value>The mod manager to use to manage mods.</value>
		public ModManager ModManager { get; private set; }

		/// <summary>
		/// Gets the mod repository from which to get mods and mod metadata.
		/// </summary>
		/// <value>The mod repository from which to get mods and mod metadata.</value>
		public IModRepository ModRepository { get; private set; }

		/// <summary>
		/// Gets the category manager to use to manage categories.
		/// </summary>
		/// <value>The category manager to use to manage categories.</value>
		public CategoryManager CategoryManager { get; private set; }

		/// <summary>
		/// Gets the list of mods being managed by the mod manager.
		/// </summary>
		/// <value>The list of mods being managed by the mod manager.</value>
		public ReadOnlyObservableList<IMod> ManagedMods
		{
			get
			{
				return ModManager.ManagedMods;
			}
		}

		/// <summary>
		/// Gets the newest available information about the managed mods.
		/// </summary>
		/// <value>The newest available information about the managed mods.</value>
		public ReadOnlyObservableList<AutoUpdater.UpdateInfo> NewestModInfo
		{
			get
			{
				return ModManager.NewestModInfo;
			}
		}

		/// <summary>
		/// Gets the list of active mods.
		/// </summary>
		/// <value>The list of active mods.</value>
		public ReadOnlyObservableList<IMod> ActiveMods
		{
			get
			{
				return ModManager.ActiveMods;
			}
		}

		/// <summary>
		/// Gets the application and user settings.
		/// </summary>
		/// <value>The application and user settings.</value>
		public ISettings Settings { get; private set; }

		/// <summary>
		/// Gets the theme to use for the UI.
		/// </summary>
		/// <value>The theme to use for the UI.</value>
		public Theme CurrentTheme { get; private set; }

		/// <summary>
		/// Gets whether the manager is in offline mode.
		/// </summary>
		/// <value>Whether the manager is in offline mode.</value>
		public bool OfflineMode
		{
			get
			{
				return ModManager.ModRepository.IsOffline;
			}
		}

		/// <summary>
		/// Gets whether the category file has been initialized.
		/// </summary>
		/// <value>Whether the category file has been initialized.</value>
		public bool IsCategoryInitialized
		{
			get
			{
				return m_booIsCategoryInitialized;
			}
		}

		public Control ParentForm
		{
			get
			{
				return m_ctlParentForm;
			}
			set
			{
				m_ctlParentForm = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// A simple constructor that initializes the object with its dependencies.
		/// </summary>
		/// <param name="p_mmdModManager">The mod manager to use to manage mods.</param>
		/// <param name="p_setSettings">The application and user settings.</param>
		/// <param name="p_thmTheme">The current theme to use for the views.</param>
		public ModManagerVM(ModManager p_mmdModManager, ISettings p_setSettings, Theme p_thmTheme)
		{
			ModManager = p_mmdModManager;
			ModRepository = p_mmdModManager.ModRepository;
			Settings = p_setSettings;
			CurrentTheme = p_thmTheme;
			CategoryManager = new CategoryManager(ModManager.CurrentGameModeModDirectory, "categories");
			if (this.CategoryManager.IsValidPath)
			{
				this.CategoryManager.LoadCategories(String.Empty);
				m_booIsCategoryInitialized = true;
			}
			else
				this.CategoryManager.Backup();
			AddModCommand = new Command<string>("Add Mod", "Adds a mod to the manager.", AddMod);
			DeleteModCommand = new Command<IMod>("Delete Mod", "Deletes the selected mod.", DeleteMod);
			ActivateModCommand = new Command<IMod>("Activate Mod", "Activates the selected mod.", ActivateMod);
			DeactivateModCommand = new Command<IMod>("Deactivate Mod", "Deactivates the selected mod.", DeactivateMod);
			TagModCommand = new Command<IMod>("Tag Mod", "Gets missing mod info.", TagMod);

			ModManager.UpdateCheckStarted += new EventHandler<EventArgs<IBackgroundTask>>(ModManager_UpdateCheckStarted);
		}

		#endregion

		#region Readme Management
		/// <summary>
		/// Delete the ReadMe file.
		/// </summary>
		/// <param name="p_modMod">The mod.</param>
		public void DeleteReadMe(IMod p_modMod)
		{
			ModManager.ReadMeManager.DeleteReadMe(Path.GetFileNameWithoutExtension(p_modMod.Filename));
			ModManager.ReadMeManager.SaveReadMeConfig();
		}

		/// <summary>
		/// Checks the ReadMe file path if exists.
		/// </summary>
		/// <param name="p_modMod">The mod.</param>
		public string[] GetModReadMe(IMod p_modMod)
		{
			return ModManager.ReadMeManager.CheckModReadMe(Path.GetFileNameWithoutExtension(p_modMod.Filename));
		}

		/// <summary>
		/// Open the ReadMe file.
		/// </summary>
		/// <param name="p_modMod">The mod.</param>
		/// <param name="p_strFileName">The file to open.</param>
		public bool OpenReadMe(IMod p_modMod, string p_strFileName)
		{
			bool booResult = false;
			string strReadMe = ModManager.ReadMeManager.GetModReadMe(Path.GetFileNameWithoutExtension(p_modMod.Filename), p_strFileName);
			if (!String.IsNullOrEmpty(strReadMe))
			{
				try
				{
					System.Diagnostics.Process.Start(strReadMe);
					booResult = true;
				}
				catch
				{
					booResult = false;
				}
			}
			return booResult;
		}

		#endregion

		#region Mod Addition/Deletion

		/// <summary>
		/// Installs the specified mod.
		/// </summary>
		/// <param name="p_strPath">The path to the mod to install.</param>
		protected void AddMod(string p_strPath)
		{
			IBackgroundTask bgtAddingTask = ModManager.AddMod(p_strPath, ConfirmFileOverwrite);
		}

		/// <summary>
		/// The callback that confirm a file overwrite.
		/// </summary>
		/// <param name="p_strOldFilePath">The path to the file that is to be overwritten.</param>
		/// <param name="p_strNewFilePath">An out parameter specifying the file to to which to
		/// write the file.</param>
		/// <returns><c>true</c> if the file should be written;
		/// <c>false</c> otherwise.</returns>
		protected bool ConfirmFileOverwrite(string p_strOldFilePath, out string p_strNewFilePath)
		{
			string strNewFileName = p_strOldFilePath;
			string strExtension = Path.GetExtension(p_strOldFilePath);
			string strDirectory = Path.GetDirectoryName(p_strOldFilePath);
			for (Int32 i = 2; i < Int32.MaxValue && File.Exists(strNewFileName); i++)
				strNewFileName = Path.Combine(strDirectory, String.Format("{0} ({1}){2}", Path.GetFileNameWithoutExtension(p_strOldFilePath), i, strExtension));
			if (File.Exists(strNewFileName))
				throw new Exception("Cannot write file. Unable to find unused file name.");
			p_strNewFilePath = ConfirmModFileOverwrite(p_strOldFilePath, strNewFileName);
			return (p_strNewFilePath != null);
		}

		/// <summary>
		/// Deletes the given mod.
		/// </summary>
		/// <param name="p_modMod">The mod to activate.</param>
		public void DeleteMod(IMod p_modMod)
		{
			if (ConfirmModFileDeletion(p_modMod))
			{
				IBackgroundTaskSet btsInstall = ModManager.DeleteMod(p_modMod, ModManager.ActiveMods);
				DeletingMod(this, new EventArgs<IBackgroundTaskSet>(btsInstall));
			}
		}

		#endregion

		#region Mod Activation/Deactivation

		/// <summary>
		/// Activates the given mod.
		/// </summary>
		/// <param name="p_modMod">The mod to activate.</param>
		protected void ActivateMod(IMod p_modMod)
		{
			string strErrorMessage = ModManager.RequiredToolErrorMessage;
			if (String.IsNullOrEmpty(strErrorMessage))
			{
				IBackgroundTaskSet btsInstall = ModManager.ActivateMod(p_modMod, ConfirmModUpgrade, ConfirmItemOverwrite, ModManager.ActiveMods);
				if (btsInstall != null)
					ChangingModActivation(this, new EventArgs<IBackgroundTaskSet>(btsInstall));
			}
			else
			{
				ExtendedMessageBox.Show(ParentForm, strErrorMessage, "Required Tool not present", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// Deactivates the given mod.
		/// </summary>
		/// <param name="p_modMod">The mod to deactivate.</param>
		protected void DeactivateMod(IMod p_modMod)
		{
			IBackgroundTaskSet btsUninstall = ModManager.DeactivateMod(p_modMod, ModManager.ActiveMods);
			ChangingModActivation(this, new EventArgs<IBackgroundTaskSet>(btsUninstall));
		}

		/// <summary>
		/// Deactivates all the mods.
		/// </summary>
		/// <param name="p_rolModList">The list of Active Mods.</param>
		public void DeactivateMultipleMods(ReadOnlyObservableList<IMod> p_rolModList)
		{
			DialogResult Result = MessageBox.Show("Do you want to uninstall all the active mods?", "Deativate Mods", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (Result == DialogResult.Yes)
			{
				DeactivatingMultipleMods(this, new EventArgs<IBackgroundTask>(ModManager.DeactivateMultipleMods(p_rolModList, ConfirmUpdaterAction)));
			}
		}

		#endregion

		#region Mod Tagging

		/// <summary>
		/// Tags the given mod.
		/// </summary>
		/// <param name="p_modMod">The mod to tag.</param>
		protected void TagMod(IMod p_modMod)
		{
			if (!ModManager.ModRepository.IsOffline)
            {
                ModTaggerVM mtgTagger = new ModTaggerVM(ModManager.GetModTagger(), p_modMod, Settings, CurrentTheme);
                TaggingMod(this, new EventArgs<ModTaggerVM>(mtgTagger));
            }
            else
            {
                ModManager.Login();
                ModTaggerVM mtgTagger = new ModTaggerVM(ModManager.GetModTagger(), p_modMod, Settings, CurrentTheme);
                ModManager.AsyncTagMod(this, mtgTagger, TaggingMod);
            }
		}

		/// <summary>
		/// Updates the mod's name.
		/// </summary>
		/// <param name="p_modMod">The mod whose name is to be updated.</param>
		/// <param name="p_strNewModName">The name to which to update the mod's name.</param>
		public void UpdateModName(IMod p_modMod, string p_strNewModName)
		{
			ModInfo mifNewInfo = new ModInfo(p_modMod);
			mifNewInfo.ModName = p_strNewModName;
			p_modMod.UpdateInfo(mifNewInfo, true);
		}

		#endregion

		#region Mod Updating

		/// <summary>
		/// Checks for mod updates.
		/// </summary>
		/// <returns>Message</returns>
		/// <param name="p_booOverrideCategorySetup">Whether to just check for mods missing the Nexus Category.</param>
		public void CheckForUpdates(bool p_booOverrideCategorySetup)
		{
            List < IMod > lstModList = new List<IMod>();

            if (p_booOverrideCategorySetup)
            {
                lstModList.AddRange(from Mod in ManagedMods
                                    where ((Mod.CategoryId == 0) && (Mod.CustomCategoryId < 0))
                                    select Mod);
            }
            else
                lstModList.AddRange(ManagedMods);
            
            if (!ModRepository.IsOffline)
            {
                if (lstModList.Count > 0)
                {
                    UpdatingMods(this, new EventArgs<IBackgroundTask>(ModManager.UpdateMods(lstModList, ConfirmUpdaterAction, p_booOverrideCategorySetup)));
                }
            }
            else
            {
                ModManager.Login();
                ModManager.AsyncUpdateMods(lstModList, ConfirmUpdaterAction, p_booOverrideCategorySetup);
            }
		}

		/// <summary>
		/// Toggles the endorsement for the given mod.
		/// </summary>
		/// <param name="p_modMod">The mod to endorse/unendorse.</param>
        public void ToggleModEndorsement(IMod p_modMod, HashSet<IMod> p_hashMods, bool? p_booEnable)
        {

			string strResult = string.Empty;
			if (String.IsNullOrEmpty(p_modMod.Id))
				throw new Exception("we couldn't find a proper Nexus ID or the file no longer exists on the Nexus sites.");

			if (!ModManager.ModRepository.IsOffline)
				ModManager.ToggleModEndorsement(p_modMod);
			else
			{
				ModManager.Login();
				ModManager.AsyncEndorseMod(p_modMod);
			}
		}

		/// <summary>
		/// Toggles the mod update warning.
		/// </summary>
		/// <param name="p_hashMods">The mod list.</param>
		/// <param name="p_booEnable">Whether to enable/disable the warning or toggle it if null.</param>
		public void ToggleModUpdateWarning(HashSet<IMod> p_hashMods, bool? p_booEnable)
		{
			TogglingAllWarning(this, new EventArgs<IBackgroundTask>(ModManager.ToggleUpdateWarningTask(p_hashMods, p_booEnable, ConfirmUpdaterAction)));
		}

		#endregion

		#region Category Updating

		/// <summary>
		/// Switches the mod category.
		/// </summary>
		/// <param name="p_modMod">The mod.</param>
		/// <param name="p_intCategoryId">The new category id.</param>
		public void SwitchModCategory(IMod p_modMod, Int32 p_intCategoryId)
		{
			ModManager.SwitchModCategory(p_modMod, p_intCategoryId);
		}

		/// <summary>
		/// Resets to the repository default categories.
		/// </summary>
		public bool ResetDefaultCategories()
		{
			string strMessage = "Are you sure you want to reset to the Nexus site default categories?";
			strMessage += Environment.NewLine + Environment.NewLine + "Note: The category list will revert to the Nexus default and your downloaded mods will be automatically reassigned to the Nexus categories.";
			DialogResult Result = MessageBox.Show(strMessage, "Category reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (Result == DialogResult.Yes)
			{
				this.CategoryManager.ResetCategories(ModManager.CurrentGameModeDefaultCategories);

				SwitchModsToCategory(-1);
				CheckForUpdates(true);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Resets to the repository default categories.
		/// </summary>
		public bool ResetToUnassigned()
		{
			string strMessage = "Are you sure you want to reset all mods to the Unassigned category?";
			strMessage += Environment.NewLine + Environment.NewLine + "Note: If you're using custom categories you won't be able to revert this operation.";
			DialogResult Result = MessageBox.Show(strMessage, "Category reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (Result == DialogResult.Yes)
			{
				SwitchModsToCategory(0);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Removes all categories and set all mods to Unassigned.
		/// </summary>
		public bool RemoveAllCategories()
		{
			string strMessage = "Are you sure you want to remove all the categories and set all mods to Unassigned?";
			strMessage += Environment.NewLine + Environment.NewLine + "Note: If you're using custom categories you won't be able to revert this operation.";
			DialogResult Result = MessageBox.Show(strMessage, "Category remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (Result == DialogResult.Yes)
			{
				CategoryManager.ResetCategories(String.Empty);
				SwitchModsToCategory(0);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Sets all managed mods to the given category id.
		/// </summary>
		/// <param name="p_intCategoryId">The unassigned category Id.</param>
		private void SwitchModsToCategory(Int32 p_intCategoryId)
		{
			UpdatingCategory(this, new EventArgs<IBackgroundTask>(CategoryManager.Update(ModManager, ManagedMods, p_intCategoryId, ConfirmUpdaterAction)));
		}

		/// <summary>
		/// Sets the selected mods to the given category id.
		/// </summary>
		/// <param name="p_lstSelectedMods">The list of selected mods.</param>
		/// <param name="p_intCategoryId">The unassigned category Id.</param>
		public void SwitchModsToCategory(List<IMod> p_lstSelectedMods, Int32 p_intCategoryId)
		{
			UpdatingCategory(this, new EventArgs<IBackgroundTask>(CategoryManager.Update(ModManager, p_lstSelectedMods, p_intCategoryId, ConfirmUpdaterAction)));
		}

		/// <summary>
		/// Sets all mods assigned to a removed category to Unassigned.
		/// </summary>
		/// <param name="p_imcCategory">The removed category.</param>
		public void SwitchModsToUnassigned(IModCategory p_imcCategory)
		{
			var CategoryMods = ManagedMods.Where(Mod => (Mod.CustomCategoryId >= 0 ? Mod.CustomCategoryId : Mod.CategoryId) == p_imcCategory.Id).ToList();

			UpdatingCategory(this, new EventArgs<IBackgroundTask>(CategoryManager.Update(ModManager, CategoryMods, 0, ConfirmUpdaterAction)));
		}

		/// <summary>
		/// Checks if the CategoryManager has been properly initialized.
		/// </summary>
		public void CheckCategoryManager()
		{
			if (!this.CategoryManager.IsValidPath)
			{
				string strMessage = "You currently don't have any file categories setup.";
				strMessage += Environment.NewLine + "Would you like NMM to organise your mods based on the categories the Nexus sites use (YES), or would you like to organise your categories yourself (NO)?";
				strMessage += Environment.NewLine + Environment.NewLine + "Note: If you choose to use Nexus categories you can still create your own categories and move your files around them. This initial Nexus setup is just a template for you to use.";

				DialogResult Result = MessageBox.Show(strMessage, "Category setup", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (Result == DialogResult.Yes)
				{
					this.CategoryManager.LoadCategories(ModManager.CurrentGameModeDefaultCategories);
					if (!OfflineMode)
						CheckForUpdates(true);
				}
				else
				{
					this.CategoryManager.LoadCategories(String.Empty);
					SwitchModsToCategory(0);
				}
			}
			else
				this.CategoryManager.LoadCategories(String.Empty);
		}

		#endregion

		#region ReadMe Manager

		/// <summary>
		/// Checks if the ReadMe Manager has been properly initialized.
		/// </summary>
		public void CheckReadMeManager()
		{
			if ((this.ModManager.ManagedMods.Count > 0) && (!this.ModManager.ReadMeManager.IsInitialized))
			{
				string strMessage = string.Empty;
				if (ModManager.ReadMeManager.IsXMLCorrupt)
					strMessage = "An error occurred loading the ReadMeManager.xml file." + Environment.NewLine + Environment.NewLine;

				strMessage += "NMM needs to setup the Readme Manager, this could take a few minutes depending on the number of mods and archive sizes.";
				strMessage += Environment.NewLine + "Do you want to perform the Readme Manager startup scan?";
				strMessage += Environment.NewLine + Environment.NewLine + "Note: if choose not to, you will be able to perform a scan by selecting any number of mods, and choosing 'Readme Scan' in the right-click menu.";

				if (MessageBox.Show(strMessage, "Readme Manager Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					SetupReadMeManager(ModManager.ManagedMods.ToList<IMod>());
			}
		}

		/// <summary>
		/// Readme Manager setup.
		/// </summary>
		/// <returns>Message</returns>
		/// <param name="p_lstModList">The list of mods.</param>
		public void SetupReadMeManager(List<IMod> p_lstModList)
		{
			ReadMeManagerSetup(this, new EventArgs<IBackgroundTask>(ModManager.SetupReadMeManager(p_lstModList, ConfirmUpdaterAction)));
		}

		#endregion

		/// <summary>
		/// Gets the list of extensions commonly used for mod files.
		/// </summary>
		/// <returns>The list of extensions commonly used for mod files.</returns>
		public IList<string> GetModFormatExtensions()
		{
			Set<string> setModExtensions = new Set<string>(StringComparer.OrdinalIgnoreCase);
			foreach (IModFormat mftFormat in ModManager.ModFormats)
				setModExtensions.Add(mftFormat.Extension);
			setModExtensions.Add(".7z");
			setModExtensions.Add(".zip");
			setModExtensions.Add(".rar");
			return setModExtensions;
		}

		/// <summary>
		/// Handles the <see cref="ModManager.UpdateCheckStarted"/> event of the ModManager.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">An <see cref="EventArgs{IBackgroundTask}"/> describing the event arguments.</param>
		private void ModManager_UpdateCheckStarted(object sender, EventArgs<IBackgroundTask> e)
		{
			UpdatingMods(sender, e);
		}
	}
}
