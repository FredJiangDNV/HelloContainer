package container.auth

default allow = false

allow {
    input.user.roles[_] == "Admin"
}

allow {
    input.user.roles[_] == "User"
    input.request.action == "read"
    input.request.resource == "container"
}

allow {
    input.user.roles[_] == "User"
    input.request.action == "read"
    input.request.resource == "alert"
}