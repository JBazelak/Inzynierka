const uploadAttachmentModal = document.getElementById('uploadAttachmentModal');
uploadAttachmentModal.addEventListener('show.bs.modal', function (event) {
    const button = event.relatedTarget;
    const materialId = button.getAttribute('data-material-id');
    const input = uploadAttachmentModal.querySelector('#uploadMaterialId');
    input.value = materialId;
});
