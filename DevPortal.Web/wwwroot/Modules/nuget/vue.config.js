module.exports = {
  publicPath: '/nuget/',
  chainWebpack: (config) => {
    config.module
      .rule('images')
      .use('url-loader')
      .tap(options => Object.assign({}, options, { name: '[name].[ext]' }));
  },
  css: {
    extract: {
      filename: 'css/[name].css',
      chunkFilename: '[name].css',
    },
  },
  configureWebpack: {
    output: {
      filename: 'js/[name].js',
      chunkFilename: '[name].js',
    }
  }
}
