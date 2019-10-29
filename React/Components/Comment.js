import React, { Component } from 'react';
import { CSSTransition } from 'react-transition-group';
import CommentReply from "./CommentReply";
import CommentEdit from "./CommentEdit";

class Comment extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isDeleted: false,
            isDeleting: false,
            deletingStarted: false,
            replyIsOn: false,
            editIsOn: false,
            commentText: props.commentText
        };

        this.replyClick = this.replyClick.bind(this);
        this.cancelReply = this.cancelReply.bind(this);
        this.editClick = this.editClick.bind(this);
        this.cancelEdit = this.cancelEdit.bind(this);
        this.deletingClick = this.deletingClick.bind(this);
        this.deleteCancel = this.deleteCancel.bind(this);
        this.deleteYes = this.deleteYes.bind(this);
        this.changeCommentText = this.changeCommentText.bind(this);
    }
    changeCommentText(newText) {        
        return fetch("/api/CommentsApi/Changecommenttext", {
            method: "POST",
            headers: {
                "X-Requested-With": "XMLHttpRequest",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                commentId: this.props.commentId,
                commentText: newText
            })
        })
            .then(response =>
                response.ok ?
                    response.json() :
                    response.json().then(error => Promise.reject(error)))
            .then(json => {
                this.setState((prevState, props) => {
                    return {
                        commentText: newText
                    }
                });
                return Promise.resolve(json);
            });        
    }
    deletingClick() {
        this.setState((prevState, props) => {
            return {
                isDeleting: true,
                editIsOn: false
            }
        });
    }
    deleteCancel() {
        this.setState((prevState, props) => {
            return {
                isDeleting: false,
                deletingStarted: false
            }
        });
    }
    deleteYes() {
        this.setState((prevState, props) => {
            return {
                deletingStarted: true
            }
        });        
        fetch(`/api/CommentsApi/Deletecomment/${this.props.commentId}`, {
            method: "POST",
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        }).then(response =>
            response.ok ?
                response.json() :
                response.json().then(error => Promise.reject(error)))
            .then(result => {
                this.setState((prevState, props) => {
                    return {
                        deletingStarted: false,
                        isDeleted: true
                    }
                });
            }).catch(error => {
                let { message } = error;
                console.error(message);
            });
    }

    replyClick() {
        this.setState((prevState, props) => {
            return { replyIsOn: true };
        });
    }

    cancelReply() {
        this.setState((prevState, props) => {
            return { replyIsOn: false };
        });
    }

    editClick() {
        this.setState((prevState, props) => {
            return { editIsOn: true };
        });
    }

    cancelEdit() {
        this.setState((prevState, props) => {
            return { editIsOn: false };
        });
    }

    render() {

        return (
            <CSSTransition
                in={!this.state.isDeleted}
                timeout={300}
                classNames="comment"
                unmountOnExit={true}>
                <div className="blog-comment-container">
                    <CommentBody
                        currentUserId={this.props.currentUserId}
                        authorized={this.props.authorized}
                        userId={this.props.userId}
                        deletingClick={this.deletingClick}
                        isDeleting={this.state.isDeleting}
                        deleteCancel={this.deleteCancel}
                        deletingStarted={this.state.deletingStarted}
                        deleteYes={this.deleteYes}
                        userName={this.props.userName}
                        repliedOnText={this.props.repliedOnText}
                        repliedOnUserName={this.props.repliedOnUserName}
                        commentText={this.state.commentText}
                        postedDate={this.props.postedDate}
                        replyClick={this.replyClick}
                        editClick={this.editClick} />
                    <CSSTransition
                        in={this.state.replyIsOn}
                        timeout={300}
                        classNames="my-node"
                        unmountOnExit={true}>
                        <div className="blog-comment-reply">
                            <CommentReply
                                {...this.props}
                                textReplied={this.state.commentText}
                                onCancel={this.cancelReply} />
                        </div>
                    </CSSTransition>
                    <CSSTransition
                        in={this.state.editIsOn}
                        timeout={300}
                        classNames="my-node"
                        unmountOnExit={true}>
                        <div className="blog-comment-reply">
                            <CommentEdit
                                commentText={this.state.commentText}
                                changeCommentText={this.changeCommentText}
                                onCancel={this.cancelEdit}
                            />
                        </div>
                    </CSSTransition>
                </div>
            </CSSTransition>
        );
    }
}


export const CommentBody = (props) => {

    let commentRepliedAnother = props.repliedOnText != "" && props.repliedOnText !== undefined && props.repliedOnText !== null;
    let postedDate = props.postedDate.replace("T", " ");
    // let pictureSource = `/Blog/UsersPictures/${props.userId}.jpg`;
    let pictureSource = `https://graph.facebook.com/${props.userId}/picture?type=normal`;
    const onUserPictureClick = () => {
        window.op
    }
    return (
        <div className="blog-comment-body">
            <div className="blog-comment-user-name">{props.userName}</div>
            <div className="blog-comment-post-date">{postedDate}</div>
            <img className="blog-comment-user-image" src={pictureSource}></img>
            <div className="blog-comment-text">{props.repliedOnText &&
                <RepliedText repliedOnUserName={props.repliedOnUserName} repliedOnText={props.repliedOnText} />}
                {props.commentText}</div>
            <div className="blog-comment-buttons">
                {props.currentUserId != props.userId &&
                    props.authorized == true &&
                    <div onClick={props.replyClick} className="blog-comment-button">Reply</div>}
                {props.currentUserId == props.userId && props.authorized &&
                    <div onClick={props.editClick} className="blog-comment-button">Edit</div>}
                {props.currentUserId == props.userId && props.authorized &&
                    <div onClick={props.deletingClick} className="blog-comment-button blog-delete-button">Delete</div>}
            </div>
            <CSSTransition
                in={props.isDeleting}
                timeout={300}
                classNames="delete-comment-dialog"
                unmountOnExit={true}>
                <div className="blog-comment-delete-dialog-container">
                    <div className="blog-comment-delete-dialog-message">Do you want to delete this message?</div>
                    <div className="blog-comment-delete-buttons-container">
                        <div onClick={props.deleteCancel} className="blog-comment-button">Cancel</div>
                        <div onClick={props.deleteYes} className="blog-comment-button blog-delete-button">Yes{
                            props.deletingStarted &&
                            <img
                                src="/icons/loading-animation.svg"
                                className="blog-comment-delete-dialog-spinner"
                                alt="spinner" />}</div>
                    </div>
                </div>
            </CSSTransition>
        </div>
    );
}

export const RepliedText = (props) => {
    return (
        <div className="comment-body-text-replied-container">
            <div className="comment-body-text-replied-user"><b>{props.repliedOnUserName}</b> wrote: </div>
            <div className="comment-body-text-replied-text">{props.repliedOnText}</div>
        </div>
    );
}

export default Comment;
global.Comment = Comment;
global.CommentBody = CommentBody;
global.RepliedText = RepliedText;