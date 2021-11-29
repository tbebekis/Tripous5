var Demos = {};

Demos.Pager = null;
Demos.edtHtml = null;
Demos.edtJs = null;

Demos.CreateEditorPager = () => {
    Demos.Pager = new tp.TabControl('.Ace-Pager');

    Demos.edtHtml = ace.edit("edtHtml");
    Demos.edtHtml.setTheme("ace/theme/twilight");
    Demos.edtHtml.session.setMode("ace/mode/html");

    Demos.edtJs = ace.edit("edtJs");
    Demos.edtJs.setTheme("ace/theme/twilight");
    Demos.edtJs.session.setMode("ace/mode/javascript");

    let el, Code;
    el = tp('.js-code');
    if (tp.IsElement(el)) {
        Code = el.innerHTML;
        Demos.edtJs.setValue(Code, -1);
    }

    el = tp('.html-code');
    if (tp.IsElement(el)) {
        Code = el.innerHTML;
        Demos.edtHtml.setValue(Code, -1);
    }

    Demos.Pager.SelectedIndex = 1;
};