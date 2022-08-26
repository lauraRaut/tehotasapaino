// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const axios = require('axios').default;

async function uploadFile (event) {
    const file = event.target.files[0]
    axios.post('upload_file', file, {
        headers: {
            'Content-Type': file.type
        }
    })
}

//axios.





