using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class HelpWindow : Window
{

    FairyBook _book;


    protected override void OnInit()
    {
        UIPackage.AddPackage("FairGUI/MainGameUI");
        UIObjectFactory.SetPackageItemExtension("ui://MainGameUI/Book", typeof(FairyBook));
        UIObjectFactory.SetPackageItemExtension("ui://MainGameUI/Page", typeof(BookPage));
        Debug.Log("test");

        this.contentPane = UIPackage.CreateObject("MainGameUI","HelpMenuWindow").asCom;

        //Application.targetFrameRate = 60;
        //Stage.inst.onKeyDown.Add(OnKeyDown);

        Debug.Log(contentPane.GetChild("book"));
        _book = (FairyBook)contentPane.GetChild("book");
        Debug.Log(_book);
        _book.SetSoftShadowResource("ui://MainGameUI/shadow_soft");
        _book.pageRenderer = RenderPage;
        _book.pageCount = 18;
        _book.currentPage = 0;
        _book.ShowCover(FairyBook.CoverType.Front, false);
        _book.onTurnComplete.Add(OnTurnComplete);

        GearBase.disableAllTweenEffect = true;
        contentPane.GetController("bookPos").selectedIndex = 1;
        GearBase.disableAllTweenEffect = false;

        contentPane.GetChild("btnNext").onClick.Add(() =>
        {
            _book.TurnNext();
        });
        contentPane.GetChild("btnPrev").onClick.Add(() =>
        {
            _book.TurnPrevious();
        });

    }


    void OnTurnComplete()
    {


        if (_book.isCoverShowing(FairyBook.CoverType.Front))
            contentPane.GetController("bookPos").selectedIndex = 1;
        else if (_book.isCoverShowing(FairyBook.CoverType.Back))
            contentPane.GetController("bookPos").selectedIndex = 2;
        else
            contentPane.GetController("bookPos").selectedIndex = 0;
    }

    void RenderPage(int index, GComponent page)
    {
        ((BookPage)page).render(index);
    }

}


