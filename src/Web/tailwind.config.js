const colors = require('tailwindcss/colors')

module.exports = {
    mode: 'jit',
    purge: [
        './wwwroot/index.html',
        './**/*.razor',
    ],
    darkMode: 'media',
    theme: {
        extend: {
            colors: {
                cyan: colors.cyan
            }
        },
    },
    variants: {
        extend: {},
    }
}
