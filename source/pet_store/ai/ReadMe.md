
# 🐾 Pet Store AI - Flask + Gemini

Bu proje, Google Gemini API kullanarak evcil hayvan tanıtımı ve önerisi yapan basit bir Flask servisidir.

---
## Canlı Kullanım Linki:
```bash
https://unuvarx.pythonanywhere.com/
```



### `POST /generate-description`

Verilen hayvan bilgilerine göre tanıtım metni üretir.

#### Örnek Request:

```json
{
  "type": "kedi",
  "breed": "British Shorthair"
}
```

---

### `POST /recommend-pet`

Kullanıcının yaşam tarzına göre evcil hayvan önerir.

#### Örnek Request:

```json
{
  "preferences": "apartmanda yaşayan, çocuklu aile"
}
```

---

## 📦 Gerekli Paketler (`requirements.txt`)

```txt
Flask
python-dotenv
google-generativeai
```

---

## 🧠 Desteklenen AI Platformu

- [Gemini (Google Generative AI)](https://ai.google.dev/)

---

Hazırlayan: `Pet Store AI Ekibi` 🐶🐱🐰
