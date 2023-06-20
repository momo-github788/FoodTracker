export function authHeader() {
    let user = JSON.parse(localStorage.getItem('user'));

    console.log("user: ", user)

    if (user && user.accessToken) {
        console.log("there is a user logged in ", user)
        return { 'Authorization': 'Bearer ' + user.accessToken };
   
    } else {
        return {}
    }
}