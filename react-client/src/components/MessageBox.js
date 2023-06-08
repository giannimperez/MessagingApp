import React, { useState, useEffect, useRef } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

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

            // Handle bad API response
            if (!response.ok) {
                const errorData = await response.json();
                const errorMessage = errorData.Message;

                toast.error(errorMessage); // display error modal
                throw new Error(errorMessage);
            }

        } catch (error) {
            console.error(error);
        }
    }

    // send message POST
    const handleSubmit = (event) => {
        event.preventDefault(); // prevents rerenders from cancelling POST request

        sendMessage();

        setText(""); // reset textbox
    };

    // request ai message suggestion
    async function getAiMessageSuggestion() {
        try {
            const response = await fetch(
                `https://localhost:5001/api/messages/${otherUser}/aisuggestmessage`,
                {
                    method: "GET",
                    headers: {
                        Authorization: "Bearer " + userInfo.token,
                    },
                }
            );

            // Handle bad API response
            if (!response.ok) {
                const errorData = await response.json();
                const errorMessage = errorData.Message;

                toast.error(errorMessage); // display error modal
                throw new Error(errorMessage);
            }

            const responseData = await response.json();
            setText(responseData.AiMessageSuggestion);
        } catch (error) {
            console.error(error);
        }
    }

    // select textbox on render
    const inputRef = useRef(null);
    useEffect(() => {
        inputRef.current.select(); // select textbox
    }, []);

    // if enter pressed, submit form
    const checkEnterPressed = (e) => {
        try {
            if (e.keyCode == 13 && e.shiftKey == false) {
                e.preventDefault();
                handleSubmit(e);
            }
        } catch (error) {
            console.error(error);
        }
    }

    return (
        <div className="message-box">
            <button className="message-box-ai-suggestion-button" onClick={getAiMessageSuggestion}>
                <img src="./AiIcon.png" alt="AI"/>
            </button>
            <form onSubmit={handleSubmit} onKeyDown={checkEnterPressed}>
                <textarea
                    ref={inputRef}
                    className="message-box-entry"
                    type="text"
                    value={text}
                    placeholder="Write a message"
                    onChange={(event) => setText(event.target.value)}
                    required
                />
                <input className="message-box-button" type="submit" value="Send" />
                <ToastContainer
                    position="bottom-right"
                    autoClose={3000}
                    hideProgressBar
                    newestOnTop={false}
                    closeOnClick
                    rtl={false}
                    pauseOnFocusLoss
                    draggable={false}
                    pauseOnHover={true}
                    theme="colored"
                />
            </form>
        </div>
    );
}

export default MessageBox;
