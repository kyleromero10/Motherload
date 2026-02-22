using Godot;
using System;

public partial class Ui : Control
{
    [Export] public Button start;
    [Export] public Button menu;
    [Export] public ItemList items;
    [Export] public Label gold;
    [Export] public Label health;
    [Export] public Start startScript;
    [Export] public BuildManager buildManager;
    

    public void OnStartPressed()
    {
        startScript.Lit();
    }

    public void OnMenuPressed()
    {
        if(items.Visible)
            items.Visible = false;
        else
        items.Visible = true;
    }

    public void OnItemSelected(int index)
    {
        buildManager.activeObject = index+1;
        buildManager.ChangePreview();
    }

}
