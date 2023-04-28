tinymce.PluginManager.add('relnews', function (editor, url) {
    // Add a button that opens a window
    editor.addButton('relnews', {
        image: '/tinymce/plugins/relnews/rel.png',
        icon: true,
        onclick: function () {
            // Open window
            editor.windowManager.open({
                title: 'Chèn box bài viết liên quan',
                width: 1000,
                height: 500,
                body: [{
                    type: 'container',
                    html:
                       
                        "<div>Từ khóa tìm kiếm</div>\r\n" +
                        "<table>\r\n" +
                        "   <tr>\r\n"+
                        "       <td><input onkeypress=\"if(event.keyCode==13)doSearchMceRelationSearch();\" style=\"border:solid 1px;padding:4px\" type=textbox id=txtSearchKeyword class=\"form-control\"></td>\r\n"+
                        "       <td><input onclick=\"javascript:doSearchMceRelationSearch();\" style=\"margin-left:10px; height:20px; text-align:center; width:80px; border:solid 1px black; padding:2px\" type=button class=\"btn blue\" value=\"Tìm kiếm\"></td>\r\n" +
                        "   </tr>\r\n" +
                        "</table>\r\n" +
                        "<div style=\"margin-bottom:10px;margin-top:10px\"><select class=\"form-control\" style=\"border:solid 1px silver\" id=drpSelectType><option value='Left'>Box căn trái</option><option value='Right'>Box căn phải</option><option value='Center'>Box căn giữa</option></select></div>" +
                        "<div style=\"margin-bottom:10px\"><select class=\"form-control\" style=\"border:solid 1px silver\" id=drpSelectImage><option value='0'>Không có ảnh đại diện</option><option value='1'>Có ảnh đại diện</option></select></div>" +
                        "<div id=\"divSearchListResultMce\" style=\"height:600px;width:750px\" style\"color:black\"></div>"
                }],
                
                onsubmit: function (e) {
                    // Insert content when the window form is submitted
                    CallAddSelectRelationToMce(editor);
                }
            });
        }
    });

});