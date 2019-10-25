import React, { Component } from "react";

class SearchBar extends Component {
    constructor(props) {
        super(props);
        this.state = {
            searchText: ""
        }
        this.changeSearchText = this.changeSearchText.bind(this);
    }
    changeSearchText(e) {
        let newValue = e.target.value;
        this.props.changeFilterText(newValue);
    }

    render() {
        return (
            <div className="blog-search-bar">
                <div className="blog-search-bar-inner">
                    <input className="blog-search-bar-input"
                        onChange={this.changeSearchText}
                        type="text"
                        spellCheck="false"
                        placeholder="search posts..." />
                    <img className="blog-search-bar-icon" src="/icons/icons8-search.svg" width="40" />
                </div>
            </div>
        );
    }
}

export default SearchBar;
global.SearchBar = SearchBar;