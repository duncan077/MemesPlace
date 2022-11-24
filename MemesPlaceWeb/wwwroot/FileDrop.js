export function initializeFileDropZone(element, inputFile) {
    function pasteHandler(e) {
        inputFile.files = e.clipboardData.files;
        const event = new Event('change', { bubbles: true });
        inputFile.dispatchEvent(event);
    }

   

    element.addEventListener('paste', pasteHandler);


    return {
        dispose: () => {
            element.removeEventListener('paste', pasteHandler);
            
        }
    }
}