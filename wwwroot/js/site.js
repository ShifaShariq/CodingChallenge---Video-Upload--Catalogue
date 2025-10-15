document.addEventListener("DOMContentLoaded", function () {
    const uploadTab = document.getElementById("uploadTab");
    const catalogueTab = document.getElementById("catalogueTab");
    const tabContent = document.getElementById("tab-content");
    const navLinks = document.querySelectorAll(".nav-link");

    function setActiveTab(clickedTab) {
        navLinks.forEach(link => link.classList.remove("active"));
        clickedTab.classList.add("active");
    }

    function loadPartial(url) {
        fetch(url)
            .then(response => {
                if (!response.ok) throw new Error("Network response was not ok");
                return response.text();
            })
            .then(html => {
                tabContent.innerHTML = html;
                bindUploadForm(); // Rebind after content is loaded
            })
            .catch(error => {
                tabContent.innerHTML = `<div class="text-danger">Error loading content: ${error.message}</div>`;
            });
    }

    uploadTab.addEventListener("click", function (e) {
        e.preventDefault();
        setActiveTab(this);
        loadPartial("/Home/UploadPartial");
    });

    catalogueTab.addEventListener("click", function (e) {
        e.preventDefault();
        setActiveTab(this);
        loadPartial("/Home/CataloguePartial");
    });

    bindUploadForm(); // Initial bind

    function bindUploadForm() {
        const form = document.getElementById('videoUploadForm');
        if (!form) return;

        form.addEventListener('submit', function (e) {
            e.preventDefault();
            const errorDiv = document.getElementById("error-display");
            errorDiv.innerText = '';
            errorDiv.style.display = "none";

            const formData = new FormData(form);

            fetch('/Video/Upload', {
                method: 'POST',
                body: formData
            })
                .then(response => {
                    if (!response.ok) {
                        return response.text().then(message => {
                            throw new Error(`Error ${ response.status }: ${ message }`); 
                        });
                    }

                    const catalogueTab = document.getElementById('catalogueTab');
                    setActiveTab(catalogueTab);
                    loadPartial("/Home/CataloguePartial");
                })
                .catch(error => {
                    const errorDiv = document.getElementById("error-display");
                    errorDiv.innerText = error.message;
                    errorDiv.style.display = "block";

                });
        });
    }

});

function playVideo(videoSrc) {
    const videoPlayer = document.getElementById("videoPlayer");
    const videoSource = document.getElementById("videoSource");

    videoSource.src = videoSrc;
    videoPlayer.load();
    videoPlayer.play();
}


