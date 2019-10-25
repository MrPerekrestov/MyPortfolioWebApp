const path = require('path');
let Blog = {
    entry: './React/Components/Blog.js',
    module: {
        rules: [{
            test: /\.(js|jsx)$/,
            exclude: /node_modules/,
            use: {
                loader: "babel-loader"
            }
        }
        ]
    },
    resolve: {
        extensions: ['*', '.js']
    },
    output: {
        filename: 'BlogComponents.js',
        path: path.resolve(__dirname, 'wwwroot/components'),
    }
}
let Comments = {
    entry: './React/Components/Comments.js',
    module: {
        rules: [{
            test: /\.(js|jsx)$/,
            exclude: /node_modules/,
            use: {
                loader: "babel-loader"
            }
        }
        ]
    },
    resolve: {
        extensions: ['*', '.js']
    },
    output: {
        filename: 'CommentsComponents.js',
        path: path.resolve(__dirname, 'wwwroot/components'),
    }
}

module.exports = [Blog, Comments]