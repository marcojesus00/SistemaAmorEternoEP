function validateFileSize(input, maxSizeInMB) {
    var file = input.files[0];
    var maxSizeInBytes = maxSizeInMB * 1024 * 1024; // Convert maxSizeInMB to bytes

    if (file && file.size > maxSizeInBytes) {
        alert("El tamaño del archivo excede el límite permitido de " + maxSizeInMB + " MB.");
        input.value = ''; // Clear the file input 
    }
}
