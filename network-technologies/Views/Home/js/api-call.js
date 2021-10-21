const messages_container = document.getElementById("messages");
const message_input_container = document.getElementById("message-input");
const send_message_button = document.getElementById("send-message-button");
const url = "http://188.166.25.209/weather";

function show_messages(data) {
    messages_container.innerHTML = "";
    for (let i = 0; i < data["messages"].length; i++) {
        let node = document.createElement('div');
        node.textContent = data['messages'][i];
        messages_container.append(node);
    }
}

function get_messages(user_name) {
    $.ajax({
        type: 'GET',
        url: url + '?user=' + user_name,
        dataType: 'json',
        success: function (data) {
            //alert("Load was performed.");
            show_messages(data);
        },
        error: function () {
            alert("Load was NOT performed.");
        }
    });
}

function post_message(user_name, message) {
    $.ajax({
        type: 'POST',
        url: url + '?user=' + user_name + '&message=' + message,
        dataType: 'json',
        success: function (data) {
            //alert("Post was performed.");
            show_messages(data);
        },
        error: function () {
            alert("Post was NOT performed.");
        }
    });
}

function on_send_message_button_click() {
    post_message("555", message_input_container.value);
}

send_message_button.addEventListener("click", on_send_message_button_click);
//post_message("123", "hello world!");
get_messages("555");