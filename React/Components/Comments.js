import React, { Component } from 'react';
import { Parser as HtmlToReactParser } from 'html-to-react';
import PropTypes from "../../wwwroot/lib/prop-types/prop-types.min.js";
import Comment from "./Comment";
import CommentAdd from "./CommentAdd"

class Comments extends Component {
    constructor(props) {
        super(props);
        this.state = {
            authorized: props.authorized,
            currentUserId: props.currentUserId,
            commentsData: props.commentsData,
            currentUserName: props.currentUserName
        }
       
        this.addComment = this.addComment.bind(this);
        this.facebookSignInClick = this.facebookSignInClick.bind(this);
        this.facebookSignOutClick = this.facebookSignOutClick.bind(this);
    }
    addComment(commentData) {
        return fetch("/api/CommentsApi/Addcomment", {
            method: "POST",
            headers: {
                "X-Requested-With": "XMLHttpRequest",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(commentData)
        }).then(response => response.json())
            .then(result => {                
                if (result.commentId) {              
                    let newCommentsData = [...this.state.commentsData];                 
                    ({ commentId: commentData.commentId, postedDate: commentData.postedDate}=result);
                    newCommentsData.push(commentData);
                    this.setState((prevState, props) => {
                        return {
                            commentsData: newCommentsData
                        }
                    });
                    window.scrollTo(0, document.body.scrollHeight);
                    return new Promise(resolve => resolve(true));
                }                
                else {
                    console.error("Comment was not added");
                    return new Promise(resolve => resolve(false));
                }
            })

    }

    facebookSignInClick() {
        document.forms['auth-form'].submit();
    }

    facebookSignOutClick() {
        fetch("/api/Authorization/Signout", {
            method: "POST"
        }).then(response => {
            if (response.status === 200) {
                this.setState((prevState, props) => {
                    return {
                        authorized: false,
                        currentUserId: "",
                        currentUserName: ""
                    }
                });
            }
            else {
                console.error("Sign out error");
            }
        });


    }
    render() {
        let comments = this.state.commentsData.map((commentProps, index) => {
            return (<Comment
                addComment={this.addComment}
                authorized={this.state.authorized}
                currentUserId={this.state.currentUserId}
                currentUserName={this.state.currentUserName}
                {...commentProps} />);
        });

        return (
            <div>
                {comments}
                {this.state.authorized ? <CommentAdd
                    postId={this.props.postId}
                    currentUserId={this.state.currentUserId}
                    currentUserName={this.state.currentUserName}
                    addComment={this.addComment}
                    facebookSignOutClick={this.facebookSignOutClick} /> :
                    <SignIn
                        returnUri={this.props.returnUri}
                        facebookSignInClick={this.facebookSignInClick} />}
            </div>
        );
    }
}
export const SignIn = (props) => {
    return (
        <div className="comments-signin-container">
            <div className="comments-signin-container-outer">
                <div className="comments-signin-container-text">Please, sign in with Facebook to left the comment</div>
                <form method="post" name="auth-form" id="auth-form" action="/api/Authorization/Authorize">
                    <input type="hidden" name="returnUri" value={props.returnUri} />
                    <div onClick={props.facebookSignInClick}
                        className="fb-sign-in-button fb-sign-in-button-connect">Sign in with Facebook</div>
                </form>
            </div>
        </div>
    );
}

export default Comments;
global.Comments = Comments;
global.SignIn = SignIn;
