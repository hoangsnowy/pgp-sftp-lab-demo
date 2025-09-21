# Build stage
FROM node:20-alpine AS build
WORKDIR /app

# Copy package files
COPY package*.json ./

# Install all dependencies (including dev dependencies for build)
RUN npm ci

# Copy source code
COPY . .

# Build production app
RUN npm run build

# Production stage với Nginx
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Copy built app từ build stage
COPY --from=build /app/dist .

# Copy nginx config
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]