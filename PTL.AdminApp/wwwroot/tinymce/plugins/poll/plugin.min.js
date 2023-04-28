tinymce.PluginManager.add('poll', function (editor, url) {
    // Add a button that opens a window
    editor.addButton('poll', {
        image: '/tinymce/plugins/poll/poll.png',
        icon: true,
        onclick: function () {
            // Open window
            editor.windowManager.open({
                title: 'Chèn box poll',
                width: 800,
                height: 400,
                body: [{
                    type: 'container',
                    html: "Hello world!"
                }],
                
                onsubmit: function (e) {
                    // Insert content when the window form is submitted
                    editor.insertContent('<div class=\"RelBox\">Bai viet lien quan</div>');
                }
            });
        }
    });

});