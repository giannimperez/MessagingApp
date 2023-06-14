

export default function AboutPage() {

    return (
        <div className="about">
            <div className="title">
                <h1> About Messaging App </h1>
                <p>Welcome to Messaging App, a convenient and efficient communication tool that enables users to connect and chat with one another.</p>
            </div>
            <h3>AI Message Suggestions</h3>
            <p>AI Message Suggestions are powered by OpenAI, allowing us to offer AI message suggestions. This feature enhances your chat experience by providing you with context-aware suggestions based on your previous messages. Simply click the AI Message Suggestion button, customize the suggestion if necessary, then send.</p>
            <h3>Partial Username Search</h3>
            <p>Finding and connecting with other users is made easier with our partial username search functionality. Use the live search bar to locate specific users quickly, enabling you to initiate conversations easily.</p>
            <h3>Security</h3>
            <p>All requests to the API (except login/signup) require a JWT issued by the API. No messages or user data is accessible to outside forces.</p>
        </div>
    ) 
}