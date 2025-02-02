document.addEventListener('DOMContentLoaded', function () {
    var editModal = document.getElementById('editMaterialModal');
    if (editModal) {
        editModal.addEventListener('show.bs.modal', function (event) {
            var button = event.relatedTarget;

            document.getElementById('editMaterialId').value = button.getAttribute('data-material-id');
            document.getElementById('editName').value = button.getAttribute('data-material-name') || "";
            document.getElementById('editQuantity').value = button.getAttribute('data-material-quantity') || "";
            document.getElementById('editUnit').value = button.getAttribute('data-material-unit') || "";
            document.getElementById('editPricePerUnit').value = button.getAttribute('data-material-price') || "";
        });
    }
});
