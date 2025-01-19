import sys
from keras.models import load_model
from keras.preprocessing.sequence import pad_sequences
import nltk
from nltk.corpus import stopwords
from nltk.stem import WordNetLemmatizer
import pickle
from flask import Flask,request, jsonify
from flask_cors import CORS
import string

nltk.download('stopwords')
nltk.download('wordnet')

app = Flask(__name__)
CORS(app)


with open('tokenizer.pkl', 'rb') as f:
    tokenizer = pickle.load(f)

model = load_model('model.h5')
max_len = 100



punctuations_list = string.punctuation
def remove_punctuations(text):
    temp = str.maketrans('', '', punctuations_list)
    return text.translate(temp)

def remove_stopwords(text):
    stop_words = stopwords.words('english')

    imp_words = []

    for word in str(text).split():
        if word not in stop_words:

            lemmatizer = WordNetLemmatizer()
            word = lemmatizer.lemmatize(word)

            imp_words.append(word)

    output = " ".join(imp_words)

    return output

@app.route('/predict', methods=['POST'])
def predict():
    data = request.json
    input_text = data.get('text', None)

    if not input_text:
        return jsonify({"error": "No input text provided."}), 400

    input_text = remove_punctuations(input_text)
    input_text = remove_stopwords(input_text)

    test_seq = tokenizer.texts_to_sequences([input_text])
    test_padded = pad_sequences(test_seq, maxlen=max_len)
    prediction = model.predict(test_padded)
    predicted_class = int(prediction.argmax(axis=-1)[0])

    return jsonify({"prediction": predicted_class}), 200


if __name__ == "__main__":
    app.run(port=8000,  host='0.0.0.0', debug=True)

