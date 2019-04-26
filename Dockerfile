FROM node:10.13-alpine AS build
WORKDIR /src
COPY theobu-frontend/package*.json ./
RUN npm install
COPY ./theobu-frontend/ .
RUN npm run-script build

FROM nginx:stable AS final
COPY  --from=build /src/dist/theobu-frontend /var/www
COPY ./nginx.conf /etc/nginx/conf.d/default.conf