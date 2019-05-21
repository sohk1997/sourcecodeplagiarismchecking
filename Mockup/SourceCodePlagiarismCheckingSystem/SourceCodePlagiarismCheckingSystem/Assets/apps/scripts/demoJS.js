
////Load file to code mirror
var myTextArea = document.getElementById('code_editor');

var myCodeMirror = CodeMirror.fromTextArea(myTextArea, {
    lineNumbers: true,
    theme: 'icecoder',
    mode: 'javascript',
    readoreadOnly: true
});



document.addEventListener('DOMContentLoaded', function (event) {
    document.getElementById('input-file')
        .addEventListener('change', getFile);

});


function getFile(event) {
    const input = event.target
    if ('files' in input && input.files.length > 0) {
        placeFileContent(
            document.getElementById('code_editor'),
            input.files[0])
    }
}

function placeFileContent(target, file) {
    readFileContent(file).then(content => {
        target.value = content;
        myCodeMirror.setValue(content);
    }).catch(error => console.log(error))
}

function readFileContent(file) {
    const reader = new FileReader()
    return new Promise((resolve, reject) => {
        reader.onload = event => resolve(event.target.result)
        reader.onerror = error => reject(error)
        reader.readAsText(file)

    })
}
//--------------------------------------------------------------------

