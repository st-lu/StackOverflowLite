import sys
from keras.models import load_model
from keras.preprocessing.sequence import pad_sequences
import pickle
from flask import Flask,request, jsonify
from flask_cors import CORS

app = Flask(__name__)
CORS(app)


with open('tokenizer.pkl', 'rb') as f:
    tokenizer = pickle.load(f)

model = load_model('model.h5')
max_len = 100

@app.route('/predict', methods=['POST'])
def predict():
    data = request.json
    input_text = data.get('text', None)

    if not input_text:
        return jsonify({"error": "No input text provided."}), 400

    test_seq = tokenizer.texts_to_sequences([input_text])
    test_padded = pad_sequences(test_seq, maxlen=max_len)
    prediction = model.predict(test_padded)
    predicted_class = int(prediction.argmax(axis=-1)[0])

    return jsonify({"prediction": predicted_class}), 200


if __name__ == "__main__":
    app.run(port=8000, debug=True)

