using UnityEngine;
using FairyGUI;
using GateGame.Game;

public class TutoialGuide : MonoBehaviour
{
    GComponent _mainView;
    GComponent _guideLayer;

    void Start()
    {
        _mainView = this.GetComponent<UIPanel>().ui;

        if (!(GateGameManager.instance.GetLevelForCurrentScene().id == "0" &&
           !GateGameManager.instance.IsLevelCompleted("0")))
        {
            //_mainView.GetChild("n8").visible = false;
            return;
        }
        //_guideLayer = _mainView.GetChild("n8").asCom;

        //_guideLayer.GetChild("n51").onClick.Add(() =>
        //{
        //    _guideLayer.visible = false;
        //}
        //);

        _guideLayer = UIPackage.CreateObject("MainGameUI", "TutorialMenuFrame").asCom;
        _guideLayer.SetSize(GRoot.inst.width, GRoot.inst.height);
        _guideLayer.AddRelation(GRoot.inst, RelationType.Size);


        GObject bagBtn = _guideLayer.GetChild("n51");
        bagBtn.onClick.Add(() =>
        {
            _guideLayer.RemoveFromParent();
        });

        GRoot.inst.AddChild(_guideLayer); //!!Before using TransformRect(or GlobalToLocal), the object must be added first

    }
}
