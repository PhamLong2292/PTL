tinymce.PluginManager.add('m2', function (editor, url) {
    // Add a button that opens a window
    editor.addButton('m2', {
        image: '/tinymce/plugins/m2/m2.png',
        icon: true,
        onclick: function () {
            //CallInsertM2(editor);
            //editor.dom.setAttrib(tinyMCE.activeEditor.dom.select('p'), 'class', 'Cssm2ription');

            // Open window
                        editor.windowManager.open({
                            title: 'Chèn mô tả ảnh',
                            width: 1000,
                            height: 500,
                            body: [{
                                type: 'container',
                                html:
                                    "<div style=\"margin-bottom:10px\">Điền dữ liệu (ví dụ: 2)</div>"+
                                    "<div><input style=\"border:solid 1px;padding:4px;width:100%\" value=\"2\" type=textbox id=txtm2 class=\"form-control\"></div>\r\n"
                                    
                            }],
                            
                            onsubmit: function (e) {
                                // Insert content when the window form is submitted
                                CallInsertM2(editor);
                            }
                        });
        }
    });

});

            function CallInsertM2(editor) {
                data = document.getElementById("txtm2").value;
                var HtmlContent = "<sup>"+data+"</sup><span>&nbsp;</span>";
                editor.insertContent(HtmlContent);
}