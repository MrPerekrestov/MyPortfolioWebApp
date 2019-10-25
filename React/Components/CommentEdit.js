import React, { Component } from 'react';

class CommentEdit extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isSending: false,
            isDisabeled: false,
            messageText: this.props.commentText,
            originalMessageText: this.props.commentText,
            errorMessage: ""
        };

        this.onSaveChangesClick = this.onSaveChangesClick.bind(this);
        this.onCancelClick = this.onCancelClick.bind(this);
        this.onTextAreaChange = this.onTextAreaChange.bind(this);
    }

    onTextAreaChange(e) {
        let newMessageText = e.target.value;
        this.setState((prevState, props) => {
            return { messageText: newMessageText };
        });
    }

    onSaveChangesClick() {
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
        
        this.props.changeCommentText(this.state.messageText).then(response => {
            this.setState((prevState, props) => {
                return {
                    isSending: false
                }
            });          
            this.props.onCancel();
        }).catch(error => {
            this.setState((prevState, props) => {
                return {
                    errorMessage: error.message
                }
            });
        });
    }

    onCancelClick() {
        if (this.state.isDisabeled) { return; }
        this.props.onCancel();
    }

    render() {
        return (
            <div className="blog-comment-compose">
                <textarea
                    onChange={this.onTextAreaChange}
                    disabled={this.state.isDisabeled}
                    autoFocus={true}
                    placeholder="Comment text..." rows={4}
                    className="blog-comment-compose-textarea"
                    spellCheck={false}
                    value={this.state.messageText}
                />
                <div className="blog-comment-compose-buttons">
                    <div className="blog-comment-compose-error-message">{this.state.errorMessage}</div>
                    <div
                        disabled={this.state.isDisabeled}
                        onClick={this.onCancelClick}
                        className="blog-comment-button">Cancel</div>
                    <div
                        disabled={this.state.isDisabeled}
                        onClick={this.onSaveChangesClick}
                        className="blog-comment-button">Save changes</div>
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

export default CommentEdit;
global.CommentCompose = CommentEdit;