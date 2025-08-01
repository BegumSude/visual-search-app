

document.getElementById("analyzeBtn").addEventListener("click", async () => {
    const fileInput = document.getElementById("imageInput");
    const file = fileInput.files[0];
    const analyzeBtn = document.getElementById("analyzeBtn");
  
    if (!file) {
      alert("Lütfen bir görsel seçin!");
      return;
    }
  
    if (!file.type.startsWith("image/")) {
      alert("Lütfen sadece görsel dosyası seçin!");
      return;
    }
  
    const reader = new FileReader();
    reader.onload = () => {
      document.getElementById("preview").innerHTML = `<img src="${reader.result}" />`;
    };
    reader.readAsDataURL(file);
  
    const formData = new FormData();
    formData.append("image", file);
  
    analyzeBtn.disabled = true;
    analyzeBtn.textContent = "Yükleniyor...";
  
    try {
      const response = await fetch("http://localhost:5000/analyze", {
        method: "POST",
        body: formData
      });
  
      const data = await response.json();
      const resultsContainer = document.getElementById("results");
      resultsContainer.innerHTML = "";
  
      data.products.forEach((product) => {
        const card = `
          <div class="product-card">
            <img src="${product.image}" alt="${product.name}" />
            <h3>${product.name}</h3>
            <p>Fiyat: ${product.price}</p>
            <a href="${product.link}" target="_blank">Satın Al</a>
          </div>
        `;
        resultsContainer.insertAdjacentHTML("beforeend", card);
      });
  
    } catch (error) {
      console.error("Hata oluştu:", error);
      alert("Bir şeyler ters gitti. Lütfen daha sonra tekrar deneyin.");
    } finally {
      analyzeBtn.disabled = false;
      analyzeBtn.textContent = "✨ Analiz Et";
    }
  });
  