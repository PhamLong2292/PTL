
tinymce.PluginManager.add('multiuploadimg', function (editor, url) {
    // Add a button that opens a window
    editor.addButton('multiuploadimg', {
        image: '/tinymce/plugins/multiuploadimg/upload.png',
        icon: true,
        onclick: function () {
            editor.windowManager.open({
                title: 'Upload nhiều ảnh',
                width: 1000,
                height: 500,
                body: [{
                    type: 'container',
                    html:
                        "</script>\r\n" +
                        
                        "<input id=txtMax type=\"hidden\" value=\"0\">\r\n"+
                        "<div style=\"margin-bottom:10px\">Chọn ảnh upload</div>" +
                        "<form runat=server  enctype=\"multipart/form-data\">\r\n" +
                        " <div id=divUpload><input onchange=\"javascript:CallProcessFileSelect(this);\" type=\"file\" id=\"uploadFile\" name=\"uploadFile[]\" multiple/></div>\r\n" +
                        "</form>\r\n" +
                        "<div id=\"divProcessingUpload\" style=\"height:20px;color:maroon;padding:4px\"></div>\r\n"+
                        "<div id=\"divUploadFilesContent\" style=\"height:400px;overflow-y:scroll\"></div>"
                }],
                            
                onsubmit: function (e) {
                    // Insert content when the window form is submitted
                    CallProcessInsertImage(editor);
                }
            });
        }
    });

});



var Processing = false;
function CallProcessFileSelect(obj) {
    
    var files = obj.files;
    
    for (var i = 0; i < files.length - 1; i++) {
        for (var j= i + 1; j<files.length; j++) {
            if(files[i].name>files[j].name)
            {
                temp = files[i];
                files[i] = files[j];
                files[j] = temp;
            }
        }
    }

    var numFiles = files.length;
    
    document.getElementById("txtMax").value = numFiles;
    document.getElementById("divUploadFilesContent").innerHTML = "";
    var startIndex = 0;
    
    for (var iIndex = 0; iIndex < numFiles; iIndex++) {
        var file = files[iIndex];
        if (file) {
            var reader = new FileReader();
            reader.onload = (function (theFile) {
                var fileName = theFile.name;
                return function (e) {
                    //console.log(fileName);
                    //console.log(e.target.result);

                    document.getElementById("divUploadFilesContent").innerHTML = document.getElementById("divUploadFilesContent").innerHTML +
                        "<div style=\"padding:4px\" id=\"divRow" + startIndex.toString() + "\"><table style=\"width:99%\"><tr><td style=\"width:20px;padding:4px\"><a href=\"javascript:removeRow(" + startIndex.toString() + ");\">[X]</a></td><td style=\"width:100px;padding:4px\"><img style=\"width:100px\" id=\"imageHolder" + startIndex.toString() + "\" src=\"" + e.target.result + "\" /></td><td style=\"padding:4px\"><input style=\"border:solid 1px silver;padding:4px;width:98%;background-color:lightyellow\" id=\"txtInfo" + startIndex.toString() + "\" style=\"width:100%\"><div>" + fileName + "</div></td><td style=\"padding:4px;width:50px\"><input style=\"background-color:lightyellow;border:solid 1px silver;padding:4px;width:98%\" id=\"txtSortIndex" + startIndex.toString() + "\" value=\"" + (startIndex + 1) + "\"></td> </tr></table></div>\r\n";
                    startIndex++;
                };
                
            })(file);
            reader.readAsDataURL(file);
            
        }
    }

    files = null;
}


function delayFunction()
{
    document.getElementById('divProcessingUpload').innerHTML = 'Đang tiến hành upload...';
}

function CallProcessInsertImage(editor) {
    if (Processing) {
        alert('Đang tiến hành upload vui lòng đợi');
        return;
    }
    Processing = true;
    
    setTimeout("delayFunction()", 1000);
    var HtmlContent = "";
    Max = parseInt(document.getElementById('txtMax').value,10);
    ORenderInfo = CreateRenderInfo();
    MultiUploadImageItems = Office.WebRender.WebScreenRender.CreateMultiUploadImageItems(ORenderInfo, Max).value;
    for(var iIndex=0;iIndex<Max;iIndex++)
    {
        MultiUploadImageItems[iIndex].HasData = 0;
        data64 = document.getElementById('imageHolder' + iIndex.toString());
        if (data64 != null) {
            src = data64.getAttribute('src');
            MultiUploadImageItems[iIndex].Data64Bits = src;
            MultiUploadImageItems[iIndex].Description = document.getElementById('txtInfo' + iIndex).value;
            MultiUploadImageItems[iIndex].HasData = 1;
            MultiUploadImageItems[iIndex].SortIndex = parseInt( document.getElementById('txtSortIndex' + iIndex).value,10);
        }
    }
    AjaxOut = Office.WebRender.WebScreenRender.ProcessMultiUploadImageToMceEditor(ORenderInfo, MultiUploadImageItems).value;
    if (AjaxOut.Error) {
        document.getElementById('divProcessingUpload').innerHTML = '';
        alert(AjaxOut.InfoMessage);
        Processing = false;
        return;
    }
    document.getElementById('divProcessingUpload').innerHTML = '';
    editor.insertContent(AjaxOut.HtmlContent);
    Processing = false;
}

function removeRow(Row) {
    if (confirm("Bạn đã chắc chắn chưa?") == false) return;
    document.getElementById("divRow" + Row).innerHTML = "";
    document.getElementById("divRow" + Row).style.display = 'none';
}