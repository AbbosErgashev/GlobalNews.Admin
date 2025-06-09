function previewMedia(input) {
    const file = input.files[0];
    const previewContainer = document.getElementById("mediaPreview");
    previewContainer.innerHTML = "";

    if (!file) return;

    const ext = file.name.split('.').pop().toLowerCase();
    const url = URL.createObjectURL(file);

    if (["jpg", "jpeg", "png", "gif"].includes(ext)) {
        const img = document.createElement("img");
        img.src = url;
        img.width = 100;
        img.height = 100;
        img.style.objectFit = "cover";
        img.style.cursor = "pointer";
        img.onclick = () => openModalMedia(url, "image");
        previewContainer.appendChild(img);
    } else if (ext === "mp4") {
        const video = document.createElement("video");
        video.width = 100;
        video.height = 100;
        video.controls = true;
        video.style.cursor = "pointer";
        video.onclick = () => openModalMedia(url, "video");

        const source = document.createElement("source");
        source.src = url;
        source.type = "video/mp4";

        video.appendChild(source);
        previewContainer.appendChild(video);
    } else {
        previewContainer.innerText = "Preview not supported for this file type.";
    }
}

function openModalMedia(url, type) {
    const modalContent = document.getElementById("modalMediaContent");
    modalContent.innerHTML = "";

    if (type === "image") {
        const img = document.createElement("img");
        img.src = url;
        img.style.width = "100%";
        img.style.maxWidth = "700px";
        img.style.height = "500px";
        img.style.objectFit = "contain";
        modalContent.appendChild(img);
    } else if (type === "video") {
        const video = document.createElement("video");
        video.src = url;
        video.controls = true;
        video.style.width = "100%";
        video.style.maxWidth = "700px";
        video.style.height = "500px";
        video.style.objectFit = "contain";
        modalContent.appendChild(video);
    }

    const modal = new bootstrap.Modal(document.getElementById("mediaModal"));
    modal.show();
}