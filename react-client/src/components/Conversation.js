import { useState, useEffect } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function Conversation() {
    // users in conversation
    const userInfo = JSON.parse(localStorage.getItem("user-info"));
    let otherUser = JSON.parse(localStorage.getItem("current-conversation-user"));

    // default amount of messages to display
    const defaultMessageRange = 20;

    // list of messages in conversation
    let [messageList, setMessageList] = useState([]);

    // number of messages to be displayed
    let [messageRange, setMessageRange] = useState(defaultMessageRange);

    // increase message range to load more messages
    function increaseMessageRange() {
        setMessageRange(messageRange + defaultMessageRange);
    }

    // gets conversation and assigns MessageList
    async function getConversation() {
        try {
            const response = await fetch(
                `https://localhost:5001/api/messages/${otherUser}/${messageRange}/conversation`,
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

            setMessageList(await response.json());
        } catch (error) {
            console.error(error);
        }
    }

    // request conversations every "interval" milliseconds
    useEffect(() => {
        getConversation(); // get conversation on load

        const interval = setInterval(getConversation, 2000); // get conversation at interval

        return () => clearInterval(interval); // clean up
    }, [messageRange]);

    return (
        <div>
            {/*Only display name for the first few messages*/}
            {messageList.length < defaultMessageRange && (
                <p className="conversation-user-title">{otherUser}</p>
            )}
            {/*Only display load more button when at defaultMessageRange*/}
            {messageList.length >= messageRange && (
                <button className="load-more-button" onClick={increaseMessageRange}>
                    Load More
                </button>
            )}
            {
                // if list contains message(s), add to map
                messageList.length >= 1
                    ? messageList.map((message, index) => {
                        {
                            // returns user sent messages, to be displayed on right
                            if (message.sender == userInfo.username) {
                                return (
                                    <div key={index}>
                                        <div className="message-from-user">
                                            <div className="message-bubble">
                                                <p className="message-text"> {message.text} </p>
                                            </div>
                                            <p className="message-date">
                                                {new Date(message.createDate).toLocaleTimeString()}{" "}
                                                {new Date(message.createDate).toDateString()}
                                            </p>
                                        </div>
                                    </div>
                                );
                            } else {
                                // returns other user messages, to be displayed on left
                                return (
                                    <div key={index}>
                                        <div className="message-from-other-user">
                                            <div className="message-bubble">
                                                <p className="message-text"> {message.text} </p>
                                            </div>
                                            <p className="message-date">
                                                {new Date(message.createDate).toLocaleTimeString()}{" "}
                                                {new Date(message.createDate).toDateString()}
                                            </p>
                                        </div>
                                    </div>
                                );
                            }
                        }
                    })
                    : ""
            }
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
        </div>
    );
}

export default Conversation;
