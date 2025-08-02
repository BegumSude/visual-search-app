from flask import Flask, request, jsonify
import json
import difflib

app = Flask(__name__)


with open("products.json", "r", encoding="utf-8") as f:
    PRODUCTS = json.load(f)


def find_similar_products(description, top_n=5):
    scored_products = []
    for product in PRODUCTS:
        score = difflib.SequenceMatcher(None, description.lower(), product["description"].lower()).ratio()
        scored_products.append((score, product))


    scored_products.sort(reverse=True, key=lambda x: x[0])
    top_matches = [p[1] for p in scored_products[:top_n]]
    return top_matches


@app.route("/get_similar_products", methods=["POST"])
def get_products():
    data = request.get_json()
    user_desc = data.get("description", "")
    if not user_desc:
        return jsonify({"error": "description not provided"}), 400

    similar_products = find_similar_products(user_desc)
    return jsonify(similar_products), 200

@app.route("/", methods=["GET"])
def home():
    return "Flask çalışıyor "

if __name__ == "__main__":
    app.run(debug=True)
