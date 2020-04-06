﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_ExosuitFabricator : NetTab
{
	[SerializeField] private GUI_ExosuitFabricatorPageMaterialsAndCategory materialsDisplay;

	private NetPageSwitcher nestedSwitcher;
	private NetPageSwitcher NestedSwitcher => nestedSwitcher ? nestedSwitcher : nestedSwitcher = this["LeftDisplay"] as NetPageSwitcher;

	private ExosuitFabricator exosuitFabricator;

	protected override void InitServer()
	{
		foreach (NetPage page in NestedSwitcher.Pages)
		{
			page.GetComponent<GUI_ExosuitFabricatorPage>().Init();
		}
	}

	private void Start()
	{
		//Makes sure it connects with the ExosuitFabricator
		exosuitFabricator = Provider.GetComponentInChildren<ExosuitFabricator>();
		//Subscribes to the MaterialsManipulated event
		ExosuitFabricator.MaterialsManipulated += UpdateAll;

		materialsDisplay.initMaterialList(exosuitFabricator);
		UpdateAll();
	}

	//Updates the GUI
	public void UpdateAll()
	{
		materialsDisplay.UpdateMaterialCount(exosuitFabricator);
	}

	public void DispenseSheet(ExoFabRemoveMaterialButton button)
	{
		Logger.Log(button.Value);
	}

	//Used by buttons to remove sheets from the exofab
	public void RemoveOneSheet(ItemTrait materialType)
	{
		exosuitFabricator.DispenseMaterialSheet(1, materialType);
	}

	//Used by buttons to remove sheets from the exofab
	public void RemoveTenSheets(ItemTrait materialType)
	{
		exosuitFabricator.DispenseMaterialSheet(10, materialType);
	}

	//Used by buttons to remove sheets from the exofab
	public void RemoveFiftySheets(ItemTrait materialType)
	{
		exosuitFabricator.DispenseMaterialSheet(50, materialType);
	}

	public void OpenTab(NetPage pageToOpen)
	{
		NestedSwitcher.SetActivePage(pageToOpen);
		pageToOpen.GetComponent<GUI_ExosuitFabricatorPage>().OpenTab();
	}

	public void CloseTab()
	{
		ControlTabs.CloseTab(Type, Provider);
	}

	private void OnDestroy()
	{
		ExosuitFabricator.MaterialsManipulated -= UpdateAll;
	}
}