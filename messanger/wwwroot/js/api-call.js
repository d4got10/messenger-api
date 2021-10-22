const messages_container = document.getElementById("messages");
const users_container = document.getElementById("users");
const message_input_container = document.getElementById("message-input");
const send_message_button = document.getElementById("send-message-button");
const messagesUrl = "/messages";//"https://188.166.25.209/weather";
const usersUrl = "/users";

const user_selected_btn = "user-selected-btn";

let chosenUser = "";
let my_name = "";

$.ajaxSetup({ headers: { 'csrftoken': '{{ csrf_token() }}' } });

function init(user_name) {
    my_name = user_name;
}

function show_messages(data) {
    messages_container.innerHTML = "";
    for (let i = 0; i < data["messages"].length; i++) {
        let node = document.createElement('div');
        message = JSON.parse(data["messages"][i]);
        if (message['Email'] == my_name) {
            node.classList.add('user-me');
            node.textContent = message['Message'];
        } else {
            node.classList.add('user-other');
            node.textContent = "Собеседник: " + message['Message'];
        }
        
        messages_container.append(node);
    }
    messages_container.scrollTop = messages_container.scrollHeight;
}

function choose_user(user_name) {
    if (chosenUser != "") {
        let prev_user_button = document.getElementById(chosenUser);
        prev_user_button.classList.remove(user_selected_btn);
    }

    chosenUser = user_name;

    let new_user_button = document.getElementById(user_name);
    new_user_button.classList.add(user_selected_btn);

    get_messages(my_name, user_name);
}

function show_users(data) {
    users_container.innerHTML = "";
    for (let i = 0; i < data.length; i++) {
        let id = data[i]['Id'];
        let email = data[i]['Email'];
        let node = document.createElement('div');
        let but = document.createElement('button');

        but.id = email;
        but.classList.add("btn");
        but.classList.add("full-width");
        but.textContent = email;

        node.append(but);
        users_container.append(node);
        if (email == chosenUser || chosenUser == "" && i == 0) {
            choose_user(email);
        }
        but.addEventListener("click", function () {
            choose_user(email);
        });
    }
}

function get_messages(user_name, from_user_name) {
    $.ajax({
        type: 'GET',
        url: messagesUrl,
        dataType: 'json',
        headers: {
            "Sender": user_name,
            "Receiver": from_user_name
        },
        success: function (data) {
            //alert("Load was performed.");
            show_messages(data);
        },
        error: function () {
            alert("Load was NOT performed.");
        }
    });
}
let data123;

function post_message(user_name, receiver_name, message) {
    $.ajax({
        type: 'POST',
        url: messagesUrl,
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(message),
        headers: {
            "Sender": user_name,
            "Receiver": receiver_name
        },
        success: function (data) {
            on_post_success(user_name, receiver_name);
        },
        error: function (data) {
            if (data.status == 200)
                on_post_success(user_name, receiver_name);
            else
                alert("Post was NOT performed.");
        }
    });
}

function on_post_success(user_name, receiver_name) {
    get_messages(user_name, receiver_name);
}

function get_users(user_name) {
    $.ajax({
        type: 'GET',
        url: usersUrl,
        dataType: 'json',
        headers: {
            "Sender": user_name
        },
        success: function (data) {
            //alert("Load was performed.");
            show_users(data);
        },
        error: function () {
            alert("Users were NOT loaded.");
        }
    });
}

function update_messages() {
    setTimeout(update_messages, 5000);
    get_messages(my_name, chosenUser);
    get_users(my_name);
}

setTimeout(update_messages, 5000);

//var source = new EventSource("/sse");

//source.onopen = function (event) {
//    alert("Opened");
//}

//source.onmessage = function (event) {
//    alert(event.data);
//}

//source.onerror = function (event) {
//    alert("Closed");
//}