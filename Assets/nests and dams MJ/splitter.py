from PIL import ImageDraw, Image

def gen_rounded_rect(width, height, radius):
    result = Image.new('RGBA', (width, height))
    draw = ImageDraw.Draw(result)
    draw.rounded_rectangle(((0, 0), (width, height)), radius, fill="white")
    result.save(f"rounded_rect_{width}_{height}_{radius}.png")

gen_rounded_rect(512, 512, 256)


def create_card(path):
    r = Image.open("rounded_rect_512_512_256.png")
    orig = Image.open(f'{path}.png')
    orig = orig.resize((1024, 1024))
    w = 512
    h = 512

    for i in range(4):
        crop = orig.crop(((i // 2) * w, (i % 2) * h, (i // 2 + 1) * w, (i % 2 + 1) * h))
        icon = Image.composite(crop, r, r)
        crop.save(f"{path}_crop_{i}.png")
        icon.save(f"{path}_icon_{i}.png")
    
##create_card("beaver_cub")
##create_card("b_lady")
##create_card("b_teacher")
##create_card("b_boxer")
##create_card("b_bandit")
create_card("b_hunter")
