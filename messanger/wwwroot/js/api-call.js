const messages_container = document.getElementById("messages");
const message_input_container = document.getElementById("message-input");
const send_message_button = document.getElementById("send-message-button");
const url = "/messages";//"https://188.166.25.209/weather";

$.ajaxSetup({ headers: { 'csrftoken': '{{ csrf_token() }}' } });

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
        url: url,
        dataType: 'json',
        headers: {
            "Sender": user_name
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

function post_message(user_name, receiver_name, message) {
    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        headers: {
            "Sender": user_name,
            "Receiver": receiver_name,
            "Message": message
        },
        success: function (data) {
            //alert("Post was performed.");
            get_messages(user_name);
        },
        error: function () {
            alert("Post was NOT performed.");
        }
    });
}