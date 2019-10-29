import React, { Component } from "react";
import { Parser as HtmlToReactParser } from 'html-to-react'; 
class BlogCard extends Component {
    constructor(props) {
        super(props);
        this.click = this.click.bind(this);       
    }   
    
    click() {            
        let progressImage = document.getElementById("progress-image");
        let fetchPromise = fetch(`/blog/${this.props.id}`, {
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        });    
        ajaxFinished = false;        
        let fadeAnimationPromise = FadeInAnimation(animationDuration);

        Promise.all([fetchPromise, fadeAnimationPromise])
            .then(results => {
                ajaxFinished = true;
                return results[0].text();
            })
            .then(result => {                    
                progressImage.style.display = "none";                
                clearBlogDOM();
                let contentContainer = document.querySelector(".content-container");
                contentContainer.innerHTML = result;     
                let reactHydrateScript = document.querySelector("#react-script-container script").innerHTML;
                eval(reactHydrateScript);
                let additionalScripts = document.getElementById("blog-post-script-on-load");              
                if (additionalScripts) {                   
                    eval(additionalScripts.innerHTML);
                } 
                FadeOutAnimation(animationDuration);  
                history.pushState("", document.title, `${window.location.origin}/blog/${this.props.id}`);
                window.scrollTo(0, 0);                                
            });          
    }

    render() {
        let htmlToReactParser = new HtmlToReactParser();
        let text = htmlToReactParser.parse(this.props.description);
        let dateRegExp = /[\d]{4}\/[\d]{2}\/[\d]{2}/g;
        let published = this.props.published.replace(/-/g, "/").match(dateRegExp)[0];       
        let imgSrc = `/Blog/Logos/${this.props.logoId}.png`;
        return (
            <div onClick={this.click} className="blog-card">
                <div className="blog-card-img-and-text">
                    <img className="blog-card-img" src={imgSrc}></img>
                    <div className="blog-card-datum">{published}</div>
                    <div className="blog-card-text-outer">
                        <div className="blog-card-empty-space"></div>
                        <div className="blog-card-text-inner">
                            {text}
                        </div>
                    </div>
                </div>
                <div className="blog-card-title">{this.props.title}</div>                
            </div>
        );
    }
}

export default BlogCard;
global.BlogCard = BlogCard;




