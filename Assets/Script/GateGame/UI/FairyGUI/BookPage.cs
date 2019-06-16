using UnityEngine;
using FairyGUI;

class BookPage : GComponent
{
	Controller _style;
	GoWrapper _modelWrapper;
	GObject _pageNumber;

	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML(xml);
		
		_style = GetController("style");

		_pageNumber = GetChild("pn");

		_modelWrapper = new GoWrapper();
		_modelWrapper.supportStencil = true;
		GetChild("model").asGraph.SetNativeObject(_modelWrapper);

	}

	public void render(int pageIndex)
	{
		_pageNumber.text = (pageIndex + 1).ToString();

		if (pageIndex == 0) {
            GetChild("n28").asTextField.text = "[align=center][size=40]门电路[/size][/align]";

            _style.selectedIndex = 1; 
        }
        else if (pageIndex == 1)
        {
            GetChild("n28").asTextField.text = "[color=#][size=40]与门[/size][/color]\n输入：" +
                    	                       "[color=#FF0000]2[/color]\n输出：[color=#FF0000]1[/color]\n" +
                    	                       "输入均为红时，输出红光\n" +
                    	                       "有一输入为蓝，输出蓝光";
            _style.selectedIndex = 1;
        }
        else if (pageIndex == 2)
        {
            if (_modelWrapper.wrapTarget != null)
                Object.Destroy(_modelWrapper.wrapTarget);

            Object prefab = Resources.Load("HelpMenu/And_Same_High");
            GameObject go = (GameObject)Object.Instantiate(prefab);
            Debug.Log(prefab);
            go.transform.localPosition = new Vector3(0, 120, 1000);
            go.transform.localScale = new Vector3(100, 100, 120);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);

            GetChild("n25").asTextField.text = "两输入为红光";
            _modelWrapper.SetWrapTarget(go, true);
            _style.selectedIndex = 2;
        }
        else if (pageIndex == 3)
        {
            GetChild("n28").asTextField.text = "[color=#][size=40]与门[/size][/color]\n输入：" +
                                               "[color=#FF0000]2[/color]\n输出：[color=#FF0000]1[/color]\n" +
                                               "输入均为红时，输出红光\n" +
                                               "有一输入为蓝，输出蓝光";
            _style.selectedIndex = 1;
        }
        else if (pageIndex == 4)
        {
            if (_modelWrapper.wrapTarget != null)
                Object.Destroy(_modelWrapper.wrapTarget);

            Object prefab = Resources.Load("HelpMenu/And_Same_Low");
            GameObject go = (GameObject)Object.Instantiate(prefab);
            Debug.Log(prefab);
            go.transform.localPosition = new Vector3(0, 120, 1000);
            go.transform.localScale = new Vector3(100, 100, 120);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);

            GetChild("n25").asTextField.text = "两输入为蓝光";
            _modelWrapper.SetWrapTarget(go, true);
            _style.selectedIndex = 2;
        }
        else if (pageIndex == 5)
        {
            GetChild("n28").asTextField.text = "[color=#][size=40]与门[/size][/color]\n输入：" +
                                               "[color=#FF0000]2[/color]\n输出：[color=#FF0000]1[/color]\n" +
                                               "输入均为红时，输出红光\n" +
                                               "有一输入为蓝，输出蓝光";
            _style.selectedIndex = 1;
        }
        else if (pageIndex == 6)
        {
            if (_modelWrapper.wrapTarget != null)
                Object.Destroy(_modelWrapper.wrapTarget);

            Object prefab = Resources.Load("HelpMenu/And_Diff");
            GameObject go = (GameObject)Object.Instantiate(prefab);
            Debug.Log(prefab);
            go.transform.localPosition = new Vector3(0, 120, 1000);
            go.transform.localScale = new Vector3(100, 100, 120);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);

            GetChild("n25").asTextField.text = "两输入不同";
            _modelWrapper.SetWrapTarget(go, true);
            _style.selectedIndex = 2;
        }
        else if (pageIndex == 7)
        {
            GetChild("n28").asTextField.text = "[color=#][size=40]或门[/size][/color]\n输入：" +
                                               "[color=#FF0000]2[/color]\n输出：[color=#FF0000]1[/color]\n" +
                                               "输入均为蓝时，输出蓝光\n" +
                                               "有一输入为红，输出红光";
            _style.selectedIndex = 1;
        }
        else if (pageIndex == 8)
        {
            if (_modelWrapper.wrapTarget != null)
                Object.Destroy(_modelWrapper.wrapTarget);

            Object prefab = Resources.Load("HelpMenu/Or_Same_High");
            GameObject go = (GameObject)Object.Instantiate(prefab);
            Debug.Log(prefab);
            go.transform.localPosition = new Vector3(0, 120, 1000);
            go.transform.localScale = new Vector3(100, 100, 120);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);

            GetChild("n25").asTextField.text = "两输入为红光";
            _modelWrapper.SetWrapTarget(go, true);
            _style.selectedIndex = 2;
        }
        else if (pageIndex == 9)
        {
            GetChild("n28").asTextField.text = "[color=#][size=40]或门[/size][/color]\n输入：" +
                                               "[color=#FF0000]2[/color]\n输出：[color=#FF0000]1[/color]\n" +
                                               "输入均为蓝时，输出蓝光\n" +
                                               "有一输入为红，输出红光";
            _style.selectedIndex = 1;
        }
        else if (pageIndex == 10)
        {
            if (_modelWrapper.wrapTarget != null)
                Object.Destroy(_modelWrapper.wrapTarget);

            Object prefab = Resources.Load("HelpMenu/Or_Same_Low");
            GameObject go = (GameObject)Object.Instantiate(prefab);
            Debug.Log(prefab);
            go.transform.localPosition = new Vector3(0, 120, 1000);
            go.transform.localScale = new Vector3(100, 100, 120);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);

            GetChild("n25").asTextField.text = "两输入为蓝光";
            _modelWrapper.SetWrapTarget(go, true);
            _style.selectedIndex = 2;
        }
        else if (pageIndex == 11)
        {
            GetChild("n28").asTextField.text = "[color=#][size=40]或门[/size][/color]\n输入：" +
                                               "[color=#FF0000]2[/color]\n输出：[color=#FF0000]1[/color]\n" +
                                               "输入均为蓝时，输出蓝光\n" +
                                               "有一输入为红，输出红光";
            _style.selectedIndex = 1;
        }
        else if (pageIndex == 12)
        {
            if (_modelWrapper.wrapTarget != null)
                Object.Destroy(_modelWrapper.wrapTarget);

            Object prefab = Resources.Load("HelpMenu/Or_Diff");
            GameObject go = (GameObject)Object.Instantiate(prefab);
            Debug.Log(prefab);
            go.transform.localPosition = new Vector3(0, 120, 1000);
            go.transform.localScale = new Vector3(100, 100, 120);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);

            GetChild("n25").asTextField.text = "两输入不同";
            _modelWrapper.SetWrapTarget(go, true);
            _style.selectedIndex = 2;
        }
        else if (pageIndex == 13)
        {
            GetChild("n28").asTextField.text = "[color=#][size=40]非门[/size][/color]\n输入：" +
                                               "[color=#FF0000]1[/color]\n输出：[color=#FF0000]1[/color]\n" +
                                               "输入为蓝，输出蓝光\n" +
                                               "输入为红，输出蓝光";
            _style.selectedIndex = 1;
        }
        else if (pageIndex == 14)
        {
            if (_modelWrapper.wrapTarget != null)
                Object.Destroy(_modelWrapper.wrapTarget);

            Object prefab = Resources.Load("HelpMenu/Not_High");
            GameObject go = (GameObject)Object.Instantiate(prefab);
            Debug.Log(prefab);
            go.transform.localPosition = new Vector3(0, 120, 1000);
            go.transform.localScale = new Vector3(100, 100, 120);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);

            GetChild("n25").asTextField.text = "输入为红光";
            _modelWrapper.SetWrapTarget(go, true);
            _style.selectedIndex = 2;
        }
        else if (pageIndex == 15)
        {
            GetChild("n28").asTextField.text = "[color=#][size=40]非门[/size][/color]\n输入：" +
                                               "[color=#FF0000]1[/color]\n输出：[color=#FF0000]1[/color]\n" +
                                               "输入为蓝，输出蓝光\n" +
                                               "输入为红，输出蓝光";
            _style.selectedIndex = 1;
        }
        else if (pageIndex == 16)
        {
            if (_modelWrapper.wrapTarget != null)
                Object.Destroy(_modelWrapper.wrapTarget);

            Object prefab = Resources.Load("HelpMenu/Not_Low");
            GameObject go = (GameObject)Object.Instantiate(prefab);
            Debug.Log(prefab);
            go.transform.localPosition = new Vector3(0, 120, 1000);
            go.transform.localScale = new Vector3(100, 100, 120);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);

            GetChild("n25").asTextField.text = "输入为蓝光";
            _modelWrapper.SetWrapTarget(go, true);
            _style.selectedIndex = 2;
        }
        //      else if (pageIndex == 2)
        //{
        //	//if (_modelWrapper.wrapTarget == null)
        //	//{

        //	//	Object prefab = Resources.Load("Role/test");
        //	//	GameObject go = (GameObject)Object.Instantiate(prefab);
        //	//	go.transform.localPosition = new Vector3(0, 0, 1000);
        //	//	go.transform.localScale = new Vector3(120, 120, 120);
        //	//	go.transform.localEulerAngles = new Vector3(0, 100, 0);

        // //             GetChild("n25").asTextField.text = "测试";

        //	//	_modelWrapper.SetWrapTarget(go, true);
        //	//}

        //	_style.selectedIndex = 2; 
        //}
        else
			_style.selectedIndex = 0; 
	}
}
