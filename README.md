# TL Pokedex Api

## Required

- docker

## Running

Start the container with:

```bash
docker run --rm -it -p 5000:80 $(docker build -q .)
```

Give it some time to run. Unit tests are run as part of the build.

## Usage

I've added a swagger interface at:

- http://localhost:5000/swagger

Or you can just browse to the various URLs:

- http://localhost:5000/pokemon/mewtwo
- http://localhost:5000/pokemon/translated/mewtwo

## Further Enhancements to Consider

- Polly, for request retries
- Caching
- Detect when rate limit is hit (FunTranslations returns this as headers) and prevent further requests for a certain period
