module.exports = ({ env }) => ({
    plugins: {
        tailwindcss: {},
        autoprefixer: {},
        cssnano: env === "production" ? { preset: "default" } : false
    }
});
