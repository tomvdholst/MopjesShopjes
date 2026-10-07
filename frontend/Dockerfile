# Build stage
FROM node:22-alpine AS build
WORKDIR /src
COPY package.json package-lock.json ./
RUN npm ci
COPY . .
RUN npm run build

# Serve stage
FROM nginx:alpine AS final
COPY --from=build /src/dist/frontend/browser /usr/share/nginx/html
EXPOSE 80