import requests
import json
import random
import time


UNSPLASH_ACCESS_KEY = "ic0MueNlDVEAvvby_81Yy4Qrgy0zoXpAQWqRon_1FGg"


categories = ["bag", "dress", "shoe", "jacket", "sunglasses"]
product_names = {
    "bag": ["Black Leather Bag", "Brown Carrier Bag", "Minimalist Backpack"],
    "dress": ["Red Sundress", "Chiffon Mini Dress", "Black Night Dress"],
    "shoe": ["White Sport Shoes", "Black Stilettos", "Comfy Loafer"],
    "jacket": ["Denim Jacket", "Black Leather Jacket", "Beige Trenchcoat"],
    "sunglasses": ["Round Sunglasses", "Squared-Frame Glasses", "Retro Style Glasses"]
}


def fetch_unsplash_image(query):
    url = f"https://api.unsplash.com/photos/random?query={query}&orientation=squarish&client_id={UNSPLASH_ACCESS_KEY}"
    response = requests.get(url)
    if response.status_code == 200:
        return response.json()["urls"]["regular"]
    else:
        print(f" Couldn't pull image: {query} | Status code: {response.status_code}")
        return "https://via.placeholder.com/400"


products = []
id_counter = 1

for category in categories:
    for name in product_names[category]:
        print(f" {name} için görsel alınıyor...")
        image_url = fetch_unsplash_image(category)
        product = {
            "id": id_counter,
            "name": name,
            "category": category,
            "description": f"{name.lower()} - modern and chic design.",
            "price": f"{random.randint(299, 999)} TL",
            "image_url": image_url,
            "buy_link": f"https://fake-shop.com/buy/{id_counter}"
        }
        products.append(product)
        id_counter += 1
        time.sleep(1)


with open("products.json", "w", encoding="utf-8") as f:
    json.dump(products, f, indent=4, ensure_ascii=False)

print(" Ürün veritabanı başarıyla oluşturuldu: products.json")
