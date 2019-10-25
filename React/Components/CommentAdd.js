import React, { Component } from 'react';

class CommentAdd extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isSending: false,
            isDisabeled: false,
            messageText: "",
            errorMessage: ""
        };        
        this.onSendClick = this.onSendClick.bind(this);
        this.onTextAreaChange = this.onTextAreaChange.bind(this);
        this.onSignOutClick = this.onSignOutClick.bind(this);
    }

    onTextAreaChange(e) {
        let newMessageText = e.target.value;
        this.setState((prevState, props) => {
            return { messageText: newMessageText };
        });
    }

    onSignOutClick() {
        this.props.facebookSignOutClick();
    }

    onSendClick() {
        if (this.state.isDisabeled) { return; }
        if (this.state.originalMessageText == this.state.messageText) {
            this.props.onCancel();
            return;
        }

        if (this.state.messageText.length < 2) {
            this.setState((prevState, props) => {
                return {
                    errorMessage: "* message is too shot"
                }
            });
            return;
        }

        if (this.state.messageText.length > 5000) {
            this.setState((prevState, props) => {
                return {
                    errorMessage: "* message is too long"
                }
            });
            return;
        }

        this.setState((prevState, props) => {
            return {
                isSending: true,
                isDisabeled: true,
                errorMessage: ""
            }
        });

        let newComment = {
            postId: this.props.postId,
            userId: this.props.currentUserId,
            userName: this.props.currentUserName,
            commentText: this.state.messageText,            
        }

        this.props.addComment(newComment).then((result) => {
            this.setState((prevState, props) => {
                return {
                    isSending: false,
                    isDisabeled: false,
                    messageText: "",
                    errorMessage: ""
                }
            });
        });

    }

    render() {
        return (
            <div className="blog-comment-compose">
                <textarea
                    onChange={this.onTextAreaChange}
                    disabled={this.state.isDisabeled}                    
                    placeholder="Comment text..." rows={4}
                    className="blog-comment-compose-textarea"
                    spellCheck={false}
                    value={this.state.messageText}
                />
                <div className="blog-comment-compose-buttons">
                    <div className="blog-comment-compose-error-message">{this.state.errorMessage}</div>
                    <div
                        disabled={this.state.isDisabeled}
                        onClick={this.onSignOutClick}
                        className="blog-comment-button blog-sign-out-button">Sign out</div>
                    <div
                        disabled={this.state.isDisabeled}
                        onClick={this.onSendClick}
                        className="blog-comment-button">Send</div>
                </div>
                {this.state.isSending &&
                    <img
                        src="/icons/loading-animation.svg"
                        className="blog-comment-compose-spinner"
                        alt="spinner" />}
            </div>
        );
    }
}

export default CommentAdd;
global.CommentCompose = CommentAdd;