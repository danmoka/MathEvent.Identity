# MathEvent.Identity

---

# DESCRIPTION

    About: the authentication part of the MathEvent web application

#

## AUTHENTICATION

### USERS

#### /connect/token [POST]

    POST:
        Body (x-www-form-urlencoded) (without refresh token):
            client_id: str,
            client_secret: str,
            grant_type: password credentials,
            username: str,
            password: str,
            scope: str ("offline_access" if you want to get an refresh token),
            client_authentication: send client credentials in body
        Body (x-www-form-urlencoded) (with refresh token):
            client_id: str,
            client_secret: str,
            grant_type: refresh_token,
            refresh_token: str

#### /connect/revocation [POST]

    POST:
        Body (x-www-form-urlencoded):
            token: str (access token (reference tokens only) or refresh token)
            client_id: str
            client_secret: str

#### /connect/userinfo [GET]

    GET: return the user id (Authorization = Bearer Token)