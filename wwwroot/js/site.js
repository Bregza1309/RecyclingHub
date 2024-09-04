function toggleCalculateSection(productId) {
    const calculateSection = document.getElementById(`calculate-section-${productId}`);
    calculateSection.style.display = calculateSection.style.display === 'none' ? 'block' : 'none';
}

function Navigate(productId ,position) {
    const grossWeight = document.getElementById(`weight-${position}`).value;
    const amount = document.getElementById(`cashAmount-${position}`).value;
    const tare = document.getElementById(`tare-${position}`).value;
    const netWeight = document.getElementById(`net-${position}`).value;
    window.location.href = `home/SaleForm/${productId},${grossWeight},${amount},${tare},${netWeight}`;


}
function calculatePrice(productId) {
    const weightInput = document.getElementById(`weight-${productId}`);
    const tareInput = document.getElementById(`tare-${productId}`);
    const resultParagraph = document.getElementById(`result-${productId}`);
    const netResultParagraph = document.getElementById(`netresult-${productId}`);
    const pricePerKg = parseFloat(document.getElementById(`price-${productId}`).value);
    const amountInput = document.getElementById(`cashAmount-${productId}`);
    const netInput = document.getElementById(`net-${productId}`);
    
    const weight = parseFloat(weightInput.value);
    const tare = parseFloat(tareInput.value);
    if (!isNaN(weight) && !isNaN(tare)) {
        const netWeight = weight - tare;
        netInput.value = netWeight;
        if (!isNaN(netWeight) && netWeight >= 0) {
            // Assuming a simple calculation for demonstration purposes
            const price = netWeight * pricePerKg // 
            netResultParagraph.textContent = `Net Weight : ${netWeight}kg(s)`;
            resultParagraph.textContent = `Price: R${price.toFixed(2)}`;
            amountInput.value = price.toFixed(2);
        } else {
            resultParagraph.textContent = 'Invalid input. Please enter a valid weight.';
        }
    }
}