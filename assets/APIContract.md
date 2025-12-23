# Cookbook API

---

API для кулинарных рецептов

## Auth

---

1. `POST /cookbook/Auth/Register` - регистрация нового пользователя.

Request body:
``` json
{
  "email": "user@example.com",
  "password": "string",
  "confirmPassword": "string",
  "fullName": "string"
}
```

Response body (code: 200):
``` json
{
  "token": "string",
  "email": "user@example.com",
  "userName": null,
  "userId": 0,
  "expiresAt": "0001-01-01T00:00:00"
}
```

2. `POST /cookbook/Auth/Login` - вход в аккаунт.

Request body:
``` json
{
  "email": "user@example.com",
  "password": "string",
  "rememberMe": true
}
```

Response body (code: 200):
``` json
{
  "token": "string",
  "email": "user@example.com",
  "userName": null,
  "userId": 0,
  "expiresAt": "0001-01-01T00:00:00"
}
```

3. `GET /cookbook/Auth/Get` - получение информации о пользователе по ID.

Request body:
``` json
{
    "userId": 0
}
```

Response body (code: 200):
``` json
{
  "id": 0,
  "userName": "user@example.com",
  "email": "user@example.com",
  "emailConfirmed": false,
  "phoneNumber": null,
  "phoneNumberConfirmed": false,
  "twoFactorEnabled": false
}
```

## File

---

1. `POST /cookbook/File/Upload` - загрузка файла в базу данных Minio.

Response body (code: 200):
``` json
{
  "fileName": "string"
}
```

2. `GET /cookbook/File/Download` - получение файла из базы данных по названию.

Request body:
``` json
{
    "fileName": "string"
}
```

Response (code: 200).

3. `DELETE /cookbook/File/Delete` - удаление файла из базы данных по названию.

Request body:
``` json
{
    "fileName": "string"
}
```

Response body (code: 200):

``` json
{
  "message": "Изображение fileName удалено"
}
```

## Ingredient

---

1. `GET /cookbook/Ingredient/Get` - получение ингредиента по ID.

Request body:
``` json
{
    "id": 0
}
```

Response body (code: 200):
``` json
{
  "id": 0,
  "name": "string",
  "protein": 0,
  "fats": 0,
  "carbs": 0,
  "calories": 0
}
```

2. `GET /cookbook/Ingredient/Search` - получение ингредиента по названию.

Request body:
``` json
{
    "name": "string"
}
```

Response body (code: 200):
``` json
[
  {
    "id": 0,
    "name": "string",
    "protein": 0,
    "fats": 0,
    "carbs": 0,
    "calories": 0
  }
]
```

3. `POST /cookbook/Ingredient/Create` - создание ингредиента.

Request body:
``` json
{
  "name": "string",
  "protein": 0,
  "fats": 0,
  "carbs": 0,
  "calories": 0
}
```

Response body (code: 200):

``` json
{
  "name": "string",
  "protein": 0,
  "fats": 0,
  "carbs": 0,
  "calories": 0
}
```


## Recipe

---

1. `GET /cookbook/Recipe/Get` - получение рецепта по ID.

Response body:

``` json
{
    "id": 0
}
```

Request body (code: 200):

``` json
{
  "id": 0,
  "name": "string",
  "weight": 0,
  "servingsNumber": 0,
  "instruction": "string",
  "fileName": "string",
  "userId": 0,
  "userName": "string",
  "ingredients": [
    {
      "id": 0,
      "name": "string",
      "protein": 0,
      "fats": 0,
      "carbs": 0,
      "calories": 0,
      "weight": 0
    }
  ]
}
```

2. `GET /cookbook/Recipe/Search` - получение рецепта по названию.

Request body:

``` json
{
    "name": "string"
}
```

Response body (code: 200):

``` json
[
  {
    "id": 0,
    "name": "string",
    "weight": 0,
    "servingsNumber": 0,
    "instruction": "string",
    "fileName": "string",
    "userId": 0,
    "userName": "string",
    "ingredients": [
      {
        "id": 0,
        "name": "string",
        "protein": 0,
        "fats": 0,
        "carbs": 0,
        "calories": 0,
        "weight": 0
      }
    ]
  }
]
```

3. `POST /cookbook/Recipe/Create` - создание рецепта.

Request body:
``` json
{
  "name": "string",
  "servingsNumber": 0,
  "instruction": "string",
  "ingredients": [
    {
      "ingredientId": 0,
      "weight": 0
    }
  ],
  "fileName": "string",
  "userId": 0
}
```

Response body (code: 200):
```
{id}
```

4. `POST /cookbook/Recipe/Update` - обновление существующего рецепта. Обновить может только владелец.

Request body:

``` json
{
  "name": "string",
  "servingsNumber": 0,
  "instruction": "string",
  "ingredients": [
    {
      "ingredientId": 0,
      "weight": 0
    }
  ],
  "fileName": "string",
  "id": 0
}
```

Response (code: 200).

5. `DELETE /cookbook/Recipe/Delete` - удаление существующего рецепта. Удалить может только владелец.

Request body:

``` json
{
    "id": 0
}
```

Response (code: 200).

6. `GET /cookbook/Recipe/GetRandomFromTheMealDB` - получение случайного рецепта из TheMealDB.

Response body (code: 200):

``` json
{
  "id": 0,
  "name": "string",
  "weight": 0,
  "servingsNumber": 0,
  "instruction": "string",
  "fileName": "string",
  "userId": 0,
  "userName": "string",
  "ingredients": [
    {
      "id": 0,
      "name": "string",
      "protein": 0,
      "fats": 0,
      "carbs": 0,
      "calories": 0,
      "weight": 0
    }
  ]
}
```