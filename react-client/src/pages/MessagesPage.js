import Sidebar from '../components/Sidebar';
import Conversation from '../components/Conversation';
import MessageBox from '../components/MessageBox';

export default function MessagesPage() {

    return (
        <div className="message-page-container">
            <div className="row">
                <div className="small-column">
                    <div className="sidebar-column">
                        <Sidebar />
                    </div>
                </div>
                <div className="big-column">
                    <div className="conversation-column">
                        <Conversation />
                    </div>
                    <div className="message-box-column">
                        <MessageBox />
                    </div>
                </div>
            </div>
        </div>
    )
}