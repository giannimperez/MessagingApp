import React, { useState, useEffect, useRef } from "react";

function MessageBox() {
    // users in conversation
    const userInfo = JSON.parse(localStorage.getItem("user-info"));
    let otherUser = JSON.parse(localStorage.getItem("current-conversation-user"));

    // text to send in message
    let [text, setText] = useState("");

    // creates new message
    async function sendMessage() {
        try {
            const response = await fetch("https://localhost:5001/api/messages", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: "bearer " + userInfo.token,
                },
                body: JSON.stringify({ recipient: otherUser, text: text }),
            });
        } catch (error) {
            console.error(error);
        }
    }

    // send message POST
    const handleSubmit = (event) => {
        event.preventDefault(); // prevents rerenders from cancelling POST request

        if (text === "") {
            return;
        }

        sendMessage();

        setText(""); // reset textbox
    };

    // select textbox on render
    const inputRef = useRef(null);
    useEffect(() => {
        inputRef.current.select(); // select textbox
    }, []);

    return (
        <div className="message-box">
            <form onSubmit={handleSubmit}>
                <input
                    ref={inputRef}
                    className="message-box-entry"
                    type="text"
                    value={text}
                    placeholder="Write a message"
                    onChange={(event) => setText(event.target.value)}
                />
                <input className="message-box-button" type="submit" value="Send" />
            </form>
        </div>
    );
}

export default MessageBox;
